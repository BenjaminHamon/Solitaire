import argparse
import logging

from bhamon_development_toolkit.automation.automation_command import AutomationCommand

from automation_scripts.configuration.automation_configuration import AutomationConfiguration


logger = logging.getLogger("Main")


class InfoCommand(AutomationCommand):


    def configure_argument_parser(self, subparsers: argparse._SubParsersAction, **kwargs) -> argparse.ArgumentParser:
        return subparsers.add_parser("info", help = "show project information")


    def check_requirements(self, arguments: argparse.Namespace, **kwargs) -> None:
        pass


    def run(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        automation_configuration: AutomationConfiguration = kwargs["configuration"]

        logger.info("ProjectIdentifier: %s", automation_configuration.project_metadata.identifier)
        logger.info("ProjectDisplayName: %s", automation_configuration.project_metadata.display_name)
        logger.info("ProjectVersion: %s", automation_configuration.project_metadata.version.full_identifier)
        logger.info("Copyright: %s", automation_configuration.project_metadata.copyright_text)


    async def run_async(self, arguments: argparse.Namespace, simulate: bool, **kwargs) -> None:
        self.run(arguments, simulate = simulate, **kwargs)
