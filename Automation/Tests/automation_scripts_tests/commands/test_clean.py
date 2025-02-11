import argparse
import datetime
import os

import mockito
import pytest

from bhamon_development_toolkit.automation import automation_helpers
from bhamon_development_toolkit.automation.project_version import ProjectVersion
from bhamon_development_toolkit.python.python_package import PythonPackage

from automation_scripts.commands.clean_command import CleanCommand
from automation_scripts.configuration.automation_configuration import AutomationConfiguration
from automation_scripts.configuration.unity_development_configuration import UnityDevelopmentConfiguration
from automation_scripts.toolkit.unity.unity_project import UnityProject


def get_project_configuration():
    automation_configuration = mockito.mock(spec = AutomationConfiguration)
    automation_configuration.project_identifier = "MyProjectIdentifier" # type: ignore
    automation_configuration.project_display_name = "My Project Display Name" # type: ignore
    automation_configuration.project_version = ProjectVersion( # type: ignore
        identifier = "1.0", revision = "abcde", revision_date = datetime.datetime(2020, 1, 1), branch = None)
    automation_configuration.copyright = "Copyright (c) 2020 MyProject Contributors" # type: ignore

    automation_configuration.automation_python_package = PythonPackage( # type: ignore
        identifier = "automation-scripts", path_to_sources = "Automation/Scripts", path_to_tests = "Automation/Tests")
    automation_configuration.unity_development_configuration = UnityDevelopmentConfiguration( # type: ignore
        [ UnityProject(identifier = "MyUnityProject", path = "MyUnityProject") ])

    return automation_configuration


def create_project_files() -> None:
    os.makedirs("MyUnityProject")
    with open("MyUnityProject/MyUnityProject.sln", mode = "w", encoding = "utf-8"):
        pass


@pytest.mark.asyncio
async def test_run(tmpdir):
    with automation_helpers.execute_in_workspace(tmpdir):
        automation_configuration = get_project_configuration()
        create_project_files()

        command = CleanCommand()
        await command.run_async(argparse.Namespace(), configuration = automation_configuration, simulate = False)

        assert not os.path.exists("MyUnityProject/MyUnityProject.sln")


@pytest.mark.asyncio
async def test_run_with_simulate(tmpdir):
    with automation_helpers.execute_in_workspace(tmpdir):
        automation_configuration = get_project_configuration()
        create_project_files()

        command = CleanCommand()
        await command.run_async(argparse.Namespace(), configuration = automation_configuration, simulate = True)

        assert os.path.exists("MyUnityProject/MyUnityProject.sln")
