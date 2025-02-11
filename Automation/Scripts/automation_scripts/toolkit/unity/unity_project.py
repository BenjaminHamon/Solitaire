import dataclasses
from typing import Optional


@dataclasses.dataclass(frozen = True)
class UnityProject:
    identifier: str
    path: str
    command_namespace: Optional[str] = None
