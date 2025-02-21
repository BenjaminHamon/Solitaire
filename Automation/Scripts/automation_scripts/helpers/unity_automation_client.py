import os
from typing import Dict, Optional

from automation_scripts.toolkit.unity.unity_editor_arguments import UnityEditorArguments
from automation_scripts.toolkit.unity.unity_editor_client import UnityEditorClient


class UnityAutomationClient:


    def __init__(self, editor_client: UnityEditorClient, command_namespace: str) -> None:
        self._editor_client = editor_client
        self._command_namespace = command_namespace


    async def reimport(self,
            platform: Optional[str] = None, enable_graphics = False,
            log_file_path: Optional[str] = None, simulate: bool = False) -> None:

        build_target = self._convert_platform_to_unity_build_target(platform)

        await self._editor_client.reimport(
            build_target = build_target, enable_graphics = enable_graphics, log_file_path = log_file_path, simulate = simulate)


    async def build_application(self, # pylint: disable = too-many-arguments
            platform: str, configuration: str, asset_bundle_directory: str, package_directory: str,
            log_file_path: Optional[str] = None, simulate: bool = False) -> None:

        command = "BuildApplication"

        command_arguments = {
            "platform": platform,
            "configuration": configuration,
            "assetBundleDirectory": os.path.abspath(asset_bundle_directory),
            "outputDirectory": os.path.abspath(package_directory),
        }

        build_target = self._convert_platform_to_unity_build_target(platform)

        await self.run_editor_command(command, command_arguments, build_target, log_file_path = log_file_path, simulate = simulate)


    async def build_asset_bundles(self,
            platform: str, asset_bundle_directory: str,
            log_file_path: Optional[str] = None, simulate: bool = False) -> None:

        command = "BuildAssetBundles"

        command_arguments = {
            "platform": platform,
            "assetBundleDirectory": os.path.abspath(asset_bundle_directory),
        }

        build_target = self._convert_platform_to_unity_build_target(platform)

        await self.run_editor_command(command, command_arguments, build_target, log_file_path = log_file_path, simulate = simulate)


    async def run_editor_command(self, # pylint: disable = too-many-arguments
            command: str, command_arguments: Dict[str,str],
            build_target: Optional[str] = None, log_file_path: Optional[str] = None, simulate: bool = False) -> None:

        unity_arguments = UnityEditorArguments(batch_mode = True, enable_graphics = False, quit_on_completion = True, build_target = build_target)

        execute_method = self._command_namespace + "." + "RunEditorCommand"
        execute_method_arguments = [ "-executeMethodCommand", command ]
        execute_method_arguments += [ "-executeMethodArguments" ] + [ key + "=" + value for key, value in command_arguments.items() ]

        await self._editor_client.execute(
            unity_arguments, execute_method,  execute_method_arguments, log_file_path = log_file_path, simulate = simulate)


    def _convert_platform_to_unity_build_target(self, platform: Optional[str]) -> Optional[str]:
        if platform is None:
            return None

        if platform == "Android":
            return "Android"
        if platform == "Linux":
            return "Linux64"
        if platform == "Windows":
            return "Win64"

        raise ValueError("Unsupported platform: '%s'" % platform)
