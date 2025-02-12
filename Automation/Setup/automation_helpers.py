# cspell:words levelname

import argparse
import contextlib
import json
import logging
import os
import subprocess
import sys
from typing import Generator, Optional

import logging_helpers


@contextlib.contextmanager
def execute_in_workspace(script_path: str) -> Generator[None,None,None]:
    current_directory = os.getcwd()
    workspace_directory = resolve_workspace_root(script_path)

    os.chdir(workspace_directory)

    try:
        yield
    finally:
        os.chdir(current_directory)


def resolve_workspace_root(script_path: str) -> str:
    directory = os.path.dirname(os.path.realpath(script_path))

    while True:
        if os.path.isdir(os.path.join(directory, ".git")):
            return directory
        if os.path.dirname(directory) == directory:
            raise RuntimeError("Failed to resolve the workspace root")
        directory = os.path.dirname(directory)


def configure_logging(arguments: argparse.Namespace):
    message_format = "{asctime} [{levelname}][{name}] {message}"
    date_format = "%Y-%m-%dT%H:%M:%S"

    log_stream_verbosity: str = "info"
    log_file_path: Optional[str] = None
    log_file_verbosity: str = "debug"

    if arguments is not None and getattr(arguments, "verbosity", None) is not None:
        log_stream_verbosity = arguments.verbosity
    if arguments is not None and getattr(arguments, "log_file", None) is not None:
        log_file_path = arguments.log_file
    if arguments is not None and getattr(arguments, "log_file_verbosity", None) is not None:
        log_file_verbosity = arguments.log_file_verbosity

    logging.root.setLevel(logging.DEBUG)

    logging.addLevelName(logging.DEBUG, "Debug")
    logging.addLevelName(logging.INFO, "Info")
    logging.addLevelName(logging.WARNING, "Warning")
    logging.addLevelName(logging.ERROR, "Error")
    logging.addLevelName(logging.CRITICAL, "Critical")

    logging_helpers.configure_log_stream(logging.root, sys.stdout, log_stream_verbosity, message_format, date_format)
    if log_file_path is not None:
        logging_helpers.configure_log_file(logging.root, log_file_path, log_file_verbosity, message_format, date_format, mode = "w", encoding = "utf-8")


def create_argument_parser() -> argparse.ArgumentParser:
    argument_parser = argparse.ArgumentParser()
    argument_parser.add_argument("--simulate", action = "store_true",
        help = "perform a test run, without writing changes")
    argument_parser.add_argument("--verbosity", choices = logging_helpers.all_log_levels,
        metavar = "<level>", help = "set the logging level (%s)" % ", ".join(logging_helpers.all_log_levels))
    argument_parser.add_argument("--log-file",
        metavar = "<file_path>", help = "set the log file path")
    argument_parser.add_argument("--log-file-verbosity", choices = logging_helpers.all_log_levels,
        metavar = "<level>", help = "set the logging level for the log file (%s)" % ", ".join(logging_helpers.all_log_levels))

    return argument_parser


def load_project_configuration(workspace_directory: str) -> dict:
    project_information_file_path = os.path.join(workspace_directory, "ProjectConfiguration.json")
    with open(project_information_file_path, mode = "r", encoding = "utf-8") as project_information_file:
        project_configuration = json.load(project_information_file)

    revision = get_current_revision()
    project_configuration["ProjectVersionFull"] = project_configuration["ProjectVersionIdentifier"] + "+" + revision[:10]

    return project_configuration


def get_current_revision() -> str:
    git_command = [ "git", "rev-list", "--max-count", "1", "HEAD" ]
    git_command_result = subprocess.run(git_command, check = True, capture_output = True, text = True, encoding = "utf-8")
    return git_command_result.stdout.strip()


def log_script_information(configuration: dict, simulate: bool = False) -> None:
    logger = logging.getLogger("Main")

    if simulate:
        logger.info("(( The script is running as a simulation ))")
        logger.info("")

    logger.info("%s %s", configuration["ProjectDisplayName"], configuration["ProjectVersionFull"])
    logger.info("Script executing in '%s'", os.getcwd())
    logger.info("")
