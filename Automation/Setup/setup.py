import logging
import os
import sys

import automation_helpers
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
        run_coroutine = setup_workspace(simulate = arguments.simulate)

        asyncio_context = AsyncioContext()
        asyncio_context.run(run_coroutine)


async def setup_workspace(simulate: bool = False) -> None:
    venv_directory = ".venv-automation"
    pip_configuration_file_path = "pip.conf"

    python_system_executable = python_helpers.resolve_system_python_executable()
    venv_python_executable = python_helpers.get_venv_executable(venv_directory, "python")
    python_package_collection = [ os.path.join("Automation", "Scripts[dev]") ]

    logger.info("Setting up python virtual environment for automation (Path: %s)", venv_directory)
    await python_helpers.setup_virtual_environment(python_system_executable, venv_directory, pip_configuration_file_path, simulate = simulate)
    await python_helpers.install_python_packages_for_development(venv_python_executable, python_package_collection, simulate = simulate)


if __name__ == "__main__":
    try:
        main()
    except Exception: # pylint: disable = broad-except
        logger.error("Script failed", exc_info = True)
        sys.exit(1)
