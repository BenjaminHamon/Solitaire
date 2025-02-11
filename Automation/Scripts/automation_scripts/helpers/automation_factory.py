from bhamon_development_toolkit.processes.process_runner import ProcessRunner
from bhamon_development_toolkit.processes.process_spawner import ProcessSpawner

from automation_scripts.configuration.workspace_environment import WorkspaceEnvironment
from automation_scripts.toolkit.unity.unity_editor_client import UnityEditorClient
from automation_scripts.toolkit.unity.unity_project import UnityProject


def create_unity_editor_client(workspace_environment: WorkspaceEnvironment, unity_project: UnityProject):
    process_runner = ProcessRunner(ProcessSpawner(is_console = False))
    unity_executable = workspace_environment.get_unity_executable_from_project_path(unity_project.path)
    unity_editor_client = UnityEditorClient(process_runner, unity_executable, unity_project.path)

    return unity_editor_client
