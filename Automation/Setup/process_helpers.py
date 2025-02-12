import asyncio
import logging
import shlex
import subprocess
from typing import Any, List, Optional


def format_executable_command(command: List[str]):
    return " ".join(format_executable_command_element(element) for element in command)


def format_executable_command_element(element: str) -> str:
    return shlex.quote(element)


def run_simple(
        logger: logging.Logger, command: List[str], working_directory: Optional[str] = None, simulate: bool = False) -> None:

    logger.debug("+ %s", format_executable_command(command))

    subprocess_options = {
        "cwd": working_directory,
        "capture_output": True,
        "text": True,
        "encoding": "utf-8",
        "stdin": subprocess.DEVNULL,
    }

    if not simulate:
        result = subprocess.run(command, check = False, **subprocess_options)
        for line in result.stdout.splitlines():
            logger.debug(line)
        for line in result.stderr.splitlines():
            logger.error(line)

        if result.returncode != 0:
            raise RuntimeError("Subprocess failed (Executable: '%s', ExitCode: %s)" % (command[0], result.returncode))


async def run_simple_async(
        logger: logging.Logger,
        command: List[str],
        working_directory: Optional[str] = None,
        stdout_logging_level: int = logging.DEBUG,
        stderr_logging_level: int = logging.ERROR,
        simulate: bool = False,
        ) -> None:

    async def watch_output(stream: asyncio.StreamReader, logging_level: int) -> str:
        encoding = "utf-8"

        output = ""

        while True:
            line_as_bytes = await stream.readline()
            if not line_as_bytes:
                break

            line = line_as_bytes.decode(encoding).replace("\r\n", "\n")
            logger.log(logging_level, line.rstrip())

            output += line

        return output

    async def check_task(identifier: str, task: asyncio.Task) -> Any:
        try:
            return await asyncio.wait_for(task, 1)
        except asyncio.CancelledError:
            pass
        except asyncio.TimeoutError:
            logger.warning("Task '%s' timed out", identifier)
        except Exception: # pylint: disable = broad-except
            logger.error("Task '%s' raised an unhandled exception", identifier, exc_info = True)
        return None

    logger.debug("+ %s", format_executable_command(command))

    if not simulate:
        process = await asyncio.create_subprocess_exec(*command,
            stdin = subprocess.DEVNULL, stdout = subprocess.PIPE, stderr = subprocess.PIPE, cwd = working_directory)

        if process.stdout is None:
            raise RuntimeError("Process stdout should not be none")
        if process.stderr is None:
            raise RuntimeError("Process stderr should not be none")

        stdout_task = asyncio.create_task(watch_output(process.stdout, stdout_logging_level))
        stderr_task = asyncio.create_task(watch_output(process.stderr, stderr_logging_level))

        exit_code = await process.wait()

        if exit_code != 0:
            raise RuntimeError("Subprocess failed (Executable: '%s', ExitCode: %s)" % (command[0], exit_code))

        await check_task("stdout", stdout_task)
        await check_task("stderr", stderr_task)
