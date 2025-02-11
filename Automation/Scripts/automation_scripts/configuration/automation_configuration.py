from bhamon_development_toolkit.python.python_package import PythonPackage

from automation_scripts.configuration.project_metadata import ProjectMetadata
from automation_scripts.configuration.unity_development_configuration import UnityDevelopmentConfiguration
from automation_scripts.configuration.workspace_environment import WorkspaceEnvironment


class AutomationConfiguration:


    def __init__(self, # pylint: disable = too-many-arguments
            project_metadata: ProjectMetadata,
            unity_development_configuration: UnityDevelopmentConfiguration,
            workspace_environment: WorkspaceEnvironment,
            automation_python_package: PythonPackage) -> None:

        self.project_metadata = project_metadata
        self.unity_development_configuration = unity_development_configuration
        self.workspace_environment = workspace_environment
        self.automation_python_package = automation_python_package


    def get_artifact_default_parameters(self) -> dict:
        return {
            "project": self.project_metadata.identifier,
            "version": self.project_metadata.version.identifier,
            "revision": self.project_metadata.version.revision_short,
        }
