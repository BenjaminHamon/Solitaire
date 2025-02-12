import logging
import os
import platform
import shutil
import sys
from typing import List, Optional

import process_helpers


logger = logging.getLogger("Python")


def resolve_system_python_executable() -> str:
    if hasattr(sys, "_base_executable"):
        return sys._base_executable # type: ignore # pylint: disable = protected-access
    raise RuntimeError("Unable to resolve the system Python executable")


async def setup_virtual_environment(
        system_python_executable: str, venv_directory: str, pip_configuration_file_path: Optional[str] = None, simulate: bool = False) -> None:

    venv_python_executable = get_venv_executable(venv_directory, "python")
    if sys.executable.lower() == os.path.abspath(venv_python_executable).lower():
        raise RuntimeError("Active python is the target virtual environment")

    if os.path.isdir(venv_directory) and not simulate:
        # Try to remove the executable first since it might be in use, otherwise we would be leaving a broken venv
        if platform.system() == "Windows" and os.path.exists(os.path.join(venv_directory, "scripts", "python.exe")):
            os.remove(os.path.join(venv_directory, "scripts", "python.exe"))
        shutil.rmtree(venv_directory)

    venv_command = [ system_python_executable ]
    venv_command += [ "-m", "venv", venv_directory ]

    await process_helpers.run_simple_async(logger, venv_command, simulate = simulate)

    if pip_configuration_file_path is not None:
        pip_configuration_file_path_in_venv = _get_pip_configuration_file_path(venv_directory)
        if not simulate:
            shutil.copy(pip_configuration_file_path, pip_configuration_file_path_in_venv)

    await install_python_packages(venv_python_executable, [ "pip", "wheel" ], simulate = simulate)


def get_venv_executable(venv_directory: str, executable: str) -> str:
    if platform.system() == "Windows":
        return os.path.join(venv_directory, "scripts", executable + ".exe")
    return os.path.join(venv_directory, "bin", executable)


def _get_pip_configuration_file_path(venv_directory: str) -> str:
    if platform.system() == "Windows":
        return os.path.join(venv_directory, "pip.ini")
    return os.path.join(venv_directory, "pip.conf")


async def install_python_packages(python_executable: str, name_or_path_collection: List[str], simulate: bool = False) -> None:
    install_command = [ python_executable ]
    install_command += [ "-m", "pip", "install", "--upgrade" ] + name_or_path_collection

    await process_helpers.run_simple_async(logger, install_command, simulate = simulate)


async def install_python_packages_for_development(python_executable: str,name_or_path_collection: List[str], simulate: bool = False) -> None:

    def is_local_package(name_or_path: str) -> bool:
        return name_or_path.startswith(".") or "/" in name_or_path or "\\" in name_or_path

    install_command = [ python_executable ]
    install_command += [ "-m", "pip", "install", "--upgrade" ]

    for name_or_path in name_or_path_collection:
        install_command += [ "--editable", name_or_path ] if is_local_package(name_or_path) else [ name_or_path ]

    await process_helpers.run_simple_async(logger, install_command, simulate = simulate)
