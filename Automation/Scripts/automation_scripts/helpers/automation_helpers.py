import contextlib
import logging
import os
from typing import Generator

from bhamon_development_toolkit.automation import automation_helpers as automation_helpers_from_toolkit
from bhamon_development_toolkit.revision_control import git_helpers

# Exposing some helpers from toolkit directly
from bhamon_development_toolkit.automation.automation_helpers import configure_logging # pylint: disable = unused-import
from bhamon_development_toolkit.automation.automation_helpers import create_argument_parser # pylint: disable = unused-import
from bhamon_development_toolkit.automation.automation_helpers import create_command_instance # pylint: disable = unused-import

from automation_scripts.configuration.project_metadata import ProjectMetadata


logger = logging.getLogger("Main")


@contextlib.contextmanager
def execute_in_workspace(script_path: str) -> Generator[None,None,None]:
    workspace_root_directory = git_helpers.resolve_repository_path(script_path)
    with automation_helpers_from_toolkit.execute_in_workspace(workspace_root_directory):
        yield


def log_script_information(project_metadata: ProjectMetadata, simulate: bool = False) -> None:
    if simulate:
        logger.info("(( The script is running as a simulation ))")
        logger.info("")

    logger.info("%s %s", project_metadata.display_name, project_metadata.version.full_identifier)
    logger.info("Branch: '%s', Revision: '%s'", project_metadata.version.branch, project_metadata.version.revision)
    logger.info("Script executing in '%s'", os.getcwd())
    logger.info("")
