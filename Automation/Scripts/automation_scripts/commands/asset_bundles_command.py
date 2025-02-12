import argparse
import logging
import os
from typing import Callable, List

from bhamon_development_toolkit.automation.automation_command import AutomationCommand
from bhamon_development_toolkit.automation.automation_command_group import AutomationCommandGroup

from automation_scripts.configuration.automation_configuration import AutomationConfiguration
from automation_scripts.configuration.workspace_environment import WorkspaceEnvironment
from automation_scripts.helpers import automation_factory


logger = logging.getLogger("Main")


class AssetBundlesCommand(AutomationCommandGroup):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        local_parser: argparse.ArgumentParser = subparsers.add_parser("asset-bundles", help = "commands related to the asset bundles")

        command_collection: List[Callable[[],AutomationCommand]] = [
            _BuildCommand,
        ]

        self.add_commands(local_parser, command_collection)

        return local_parser


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


class _BuildCommand(AutomationCommand):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        local_parser: argparse.ArgumentParser = subparsers.add_parser("build", help = "build the asset bundles")
        local_parser.add_argument("--platform", required = True, metavar = "<platform>", help = "set the platform to build for")
        return local_parser


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


    def run(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        raise NotImplementedError("Not supported")


    async def run_async(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        workspace_environment: WorkspaceEnvironment = kwargs["environment"]
        automation_configuration: AutomationConfiguration = kwargs["configuration"]
        platform: str = arguments.platform

        unity_project = automation_configuration.unity_development_configuration.get_project_by_identifier("UnityClient")
        log_file_path = os.path.join("Artifacts", "Editor", "BuildAssetBundles.log")
        asset_bundle_directory = os.path.join("Artifacts", "AssetBundles", platform)

        unity_automation_client = automation_factory.create_unity_automation_client(workspace_environment, unity_project)

        await unity_automation_client.build_asset_bundles(
            platform, asset_bundle_directory, log_file_path = log_file_path, simulate = simulate)
