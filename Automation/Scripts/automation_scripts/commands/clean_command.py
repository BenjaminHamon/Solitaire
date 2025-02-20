import argparse
import glob
import logging
import os
from typing import List

from bhamon_development_toolkit.automation import automation_helpers
from bhamon_development_toolkit.automation.automation_command import AutomationCommand
from bhamon_development_toolkit.python.python_package import PythonPackage

from automation_scripts.configuration.automation_configuration import AutomationConfiguration
from automation_scripts.toolkit.unity.unity_project import UnityProject


logger = logging.getLogger("Main")


class CleanCommand(AutomationCommand):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        return subparsers.add_parser("clean", help = "clean the workspace")


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


    def run(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        automation_configuration: AutomationConfiguration = kwargs["configuration"]

        logger.info("Cleaning the workspace")
        logger.info("")

        logger.info("Cleaning artifacts")
        self.clean_artifacts("Artifacts", simulate = simulate)
        logger.debug("")

        logger.info("Cleaning Unity projects")
        self.clean_unity_project_collection(automation_configuration.unity_development_configuration.project_collection, simulate = simulate)
        logger.debug("")

        logger.info("Cleaning automation")
        self.clean_automation(automation_configuration.automation_python_package, simulate = simulate)
        logger.debug("")


    async def run_async(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        self.run(arguments, simulate = simulate, **kwargs)


    def clean_artifacts(self, artifact_directory: str, simulate: bool = False) -> None:
        automation_helpers.remove_directory(logger, artifact_directory, simulate = simulate)


    def clean_python_sources(self, python_package_collection: List[PythonPackage], simulate: bool = False) -> None:
        for python_package in python_package_collection:
            self._clean_python_package(python_package, simulate = simulate)


    def clean_python_tests(self, simulate: bool = False) -> None:
        self._clean_pytest_cache(simulate = simulate)


    def clean_unity_project_collection(self, unity_project_collection: List[UnityProject], simulate: bool = False) -> None:
        for unity_project in unity_project_collection:
            self.clean_unity_project(unity_project, simulate = simulate)


    def clean_automation(self, python_package: PythonPackage, simulate: bool = False) -> None:
        self._clean_python_package(python_package, simulate = simulate)
        self._clean_python_cache(os.path.join("Automation", "Setup"), simulate = simulate)


    def _clean_python_package(self, python_package: PythonPackage, simulate: bool = False) -> None:
        directories_to_remove = [
            os.path.join(python_package.path_to_sources, "build"),
            os.path.join(python_package.path_to_sources, "dist"),
            os.path.join(python_package.path_to_sources, python_package.name_for_file_system + ".egg-info"),
        ]

        for directory in directories_to_remove:
            automation_helpers.remove_directory(logger, directory, simulate = simulate)

        metadata_file_path = os.path.join(python_package.path_to_sources, python_package.name_for_file_system, "__metadata__.py")
        automation_helpers.remove_file(logger, metadata_file_path, simulate = simulate)

        self._clean_python_cache(python_package.path_to_sources, simulate = simulate)
        if python_package.path_to_tests is not None:
            self._clean_python_cache(python_package.path_to_tests, simulate = simulate)


    def _clean_python_cache(self, source_directory: str, simulate: bool = False) -> None:
        if not os.path.exists(source_directory):
            return

        directories_to_remove = glob.glob(os.path.join(source_directory, "**", "__pycache__"), recursive = True)

        for directory in directories_to_remove:
            automation_helpers.remove_directory(logger, directory, simulate = simulate)


    def _clean_pytest_cache(self, simulate: bool = False) -> None:
        directories_to_remove = [ ".pytest_cache" ]

        for directory in directories_to_remove:
            automation_helpers.remove_directory(logger, directory, simulate = simulate)


    def clean_unity_project(self, unity_project: UnityProject, simulate: bool = False) -> None:
        directories_to_remove = [
            os.path.join(unity_project.path, "Assets", "StreamingAssets"),
            os.path.join(unity_project.path, "Library"),
            os.path.join(unity_project.path, "Logs"),
            os.path.join(unity_project.path, "obj"),
            os.path.join(unity_project.path, "Temp"),
        ]

        files_to_remove: list[str] = [
            os.path.join(unity_project.path, "Assets", "StreamingAssets.meta"),
        ]

        files_to_remove += glob.glob(os.path.join(unity_project.path, "*.sln"))
        files_to_remove += glob.glob(os.path.join(unity_project.path, "*.csproj"))

        for directory in directories_to_remove:
            automation_helpers.remove_directory(logger, directory, simulate = simulate)
        for file_path in files_to_remove:
            automation_helpers.remove_file(logger, file_path, simulate = simulate)
