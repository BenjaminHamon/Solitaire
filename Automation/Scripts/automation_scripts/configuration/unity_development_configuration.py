from typing import List

from automation_scripts.toolkit.unity.unity_project import UnityProject


class UnityDevelopmentConfiguration:


    def __init__(self, project_collection: List[UnityProject]) -> None:
        self.project_collection = project_collection


    def get_project_by_identifier(self, identifier: str) -> UnityProject:
        project = next((x for x in self.project_collection if x.identifier == identifier), None)
        if project is None:
            raise KeyError("No Unity project matching identifier '%s'" % identifier)
        return project
