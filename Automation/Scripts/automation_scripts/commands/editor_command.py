import argparse
import logging
import os
from typing import Callable, List, Optional

from bhamon_development_toolkit.automation.automation_command import AutomationCommand
from bhamon_development_toolkit.automation.automation_command_group import AutomationCommandGroup

from automation_scripts.configuration.automation_configuration import AutomationConfiguration
from automation_scripts.configuration.workspace_environment import WorkspaceEnvironment
from automation_scripts.helpers import automation_factory


logger = logging.getLogger("Main")


class EditorCommand(AutomationCommandGroup):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        local_parser: argparse.ArgumentParser = subparsers.add_parser("editor", help = "commands related to the editor")

        command_collection: List[Callable[[],AutomationCommand]] = [
            _LaunchCommand,
            _ReimportCommand,
        ]

        self.add_commands(local_parser, command_collection)

        return local_parser


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


class _LaunchCommand(AutomationCommand):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        return subparsers.add_parser("launch", help = "launch the editor")


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


    def run(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        workspace_environment: WorkspaceEnvironment = kwargs["environment"]
        automation_configuration: AutomationConfiguration = kwargs["configuration"]

        unity_project = automation_configuration.unity_development_configuration.get_project_by_identifier("UnityClient")
        log_file_path = os.path.join("Artifacts", "Editor", "Editor.log")

        unity_editor_client = automation_factory.create_unity_editor_client(workspace_environment, unity_project)
        unity_editor_client.launch(log_file_path = log_file_path, simulate = simulate)


    async def run_async(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        self.run(arguments, simulate = simulate, **kwargs)


class _ReimportCommand(AutomationCommand):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        local_parser = subparsers.add_parser("reimport", help = "reimport all assets")
        local_parser.add_argument("--platform", metavar = "<platform>", help = "set the platform to reimport for")
        return local_parser


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


    def run(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        raise NotImplementedError("Not supported")


    async def run_async(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        workspace_environment: WorkspaceEnvironment = kwargs["environment"]
        automation_configuration: AutomationConfiguration = kwargs["configuration"]
        platform: Optional[str] = arguments.platform

        unity_project = automation_configuration.unity_development_configuration.get_project_by_identifier("UnityClient")
        log_file_path = os.path.join("Artifacts", "Editor", "Reimport.log")

        unity_automation_client = automation_factory.create_unity_automation_client(workspace_environment, unity_project)
        await unity_automation_client.reimport(platform, log_file_path = log_file_path, simulate = simulate)
