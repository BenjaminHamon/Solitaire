import dataclasses
from typing import Optional

from bhamon_development_toolkit.automation.project_version import ProjectVersion


@dataclasses.dataclass(frozen = True)
class ProjectMetadata:
    identifier: str
    display_name: str
    version: ProjectVersion

    copyright_text: Optional[str] = None

    author: Optional[str] = None
    author_email: Optional[str] = None
    project_url: Optional[str] = None
