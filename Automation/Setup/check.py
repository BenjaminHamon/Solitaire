# cspell:words pyvenv

import logging
import os
import sys

import automation_helpers
import process_helpers
import python_helpers
from asyncio_context import AsyncioContext


logger = logging.getLogger("Main")


def main() -> None:
    with automation_helpers.execute_in_workspace(__file__):
        argument_parser = automation_helpers.create_argument_parser()
        arguments = argument_parser.parse_args()

        automation_helpers.configure_logging(arguments)
        project_configuration = automation_helpers.load_project_configuration(".")

        automation_helpers.log_script_information(project_configuration, simulate = arguments.simulate)
        run_coroutine = run_checks(simulate = arguments.simulate)

        asyncio_context = AsyncioContext()
        asyncio_context.run(run_coroutine)


async def run_checks(simulate: bool = False) -> None:
    venv_directory = ".venv-automation"
    pylint_executable = python_helpers.get_venv_executable(venv_directory, "pylint")
    pytest_executable = python_helpers.get_venv_executable(venv_directory, "pytest")

    logger.info("Running linter")
    command = [ pylint_executable, "automation_scripts" ]
    await process_helpers.run_simple_async(logging.getLogger("Python"), command, stdout_logging_level = logging.DEBUG, simulate = simulate)

    logger.info("Running tests")
    command = [ pytest_executable, "--verbose", os.path.join("Automation", "Tests", "automation_scripts_tests") ]
    await process_helpers.run_simple_async(logging.getLogger("Python"), command, simulate = simulate)


if __name__ == "__main__":
    try:
        main()
    except Exception: # pylint: disable = broad-except
        logger.error("Script failed", exc_info = True)
        sys.exit(1)
