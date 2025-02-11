import dataclasses


@dataclasses.dataclass(frozen = True)
class UnityProject:
    identifier: str
    path: str
