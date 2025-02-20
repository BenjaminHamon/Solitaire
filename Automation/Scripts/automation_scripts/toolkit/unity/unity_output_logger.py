# cspell:words batchmode

import logging
import re

from bhamon_development_toolkit.processes.process_output_handler import ProcessOutputHandler


class UnityOutputLogger(ProcessOutputHandler):


    def __init__(self, logger: logging.Logger) -> None:
        self._logger = logger


    def process_stdout_line(self, line: str) -> None: # pylint: disable = too-many-branches, too-many-return-statements
        line = line.rstrip()

        log_match = re.search(r"^\[(?P<tag>.*?)\]", line)
        if log_match is not None:
            tags_to_ignore = [
                "API Updater",
                "Licensing::Client",
                "Licensing::IpcConnector",
                "Licensing::Module",
                "MODES",
                "Package Manager",
                "Physics::Module",
                "ScriptCompilation",
                "Subsystems",
                "UnityMemory",
            ]

            if log_match.group("tag") in tags_to_ignore:
                return

        log_match = re.search(r"^Player connection \[.*?\]  \* (?P<information>.*)$", line)
        if log_match is not None:
            information_matches = list(re.finditer(r"\[(?P<key>.*?)\] (?P<value>.*?)[ $]", log_match.group("information")))
            information_dict = { match.group("key"): match.group("value") for match in information_matches }
            self._logger.info("Connection from player to editor (Identifier: '%s', Address: '%s:%s')",
                information_dict["Guid"], information_dict["IP"], information_dict["Port"])
            return

        log_match = re.search(r"^Initialize engine version: (?P<version>[0-9f\.]+) \((?P<revision>[0-9a-f]+)\)$", line)
        if log_match is not None:
            self._logger.info("Initializing (UnityVersion: '%s+%s')", log_match.group("version"), log_match.group("revision"))
            return

        if line == "Batchmode quit successfully invoked - shutting down!":
            self._logger.info("Exiting")
            return

        log_match = re.search(r"^(?P<path>.*?)\((?P<line>[0-9]+),(?P<column>[0-9]+)\): error (?P<identifier>CS[0-9]+): (?P<issue>.*)$", line)
        if log_match is not None:
            self._logger.error("(%s:%s) %s: %s", log_match.group("path"), log_match.group("line"), log_match.group("identifier"), log_match.group("issue"))
            return

        log_match = re.search(r"^(?P<path>.*?)\((?P<line>[0-9]+),(?P<column>[0-9]+)\): warning (?P<identifier>CS[0-9]+): (?P<issue>.*)$", line)
        if log_match is not None:
            self._logger.warning("(%s:%s) %s: %s", log_match.group("path"), log_match.group("line"), log_match.group("identifier"), log_match.group("issue"))
            return

        if line == "Application.AssetDatabase Initial Refresh Start":
            self._logger.info("Refreshing asset database")
            return

        log_match = re.search(r"^Starting: .*\\ScriptCompilationBuildProgram\.exe", line)
        if log_match is not None:
            self._logger.info("Compiling scripts")
            return

        log_match = re.search(r"^Start importing (?P<path>.*?) using Guid\((?P<guid>[0-9a-f]+)\)", line)
        if log_match is not None:
            self._logger.debug("Importing '%s' (GUID: '%s')", log_match.group("path"), log_match.group("guid"))
            return

        log_match = re.search(r"^Compiling shader \"(?P<shader>.*?)\" pass \"(?P<pass>.*?)\" \((?P<shader_type>vp|fp)\)$", line)
        if log_match is not None:
            self._logger.debug("Compiling shader '%s' (Pass: '%s', ShaderType: '%s')",
                log_match.group("shader"), log_match.group("pass"), self._convert_shader_type_to_full_name(log_match.group("shader_type")))
            return

        log_match = re.search(r"^[a-zA-Z0-9_]*Exception: ", line)
        if log_match is not None:
            self._logger.error(line)
            return

        log_match = re.search(r"^executeMethod method (?P<method>.*) threw exception.$", line)
        if log_match is not None:
            self._logger.error("ExecuteMethod '%s' threw an exception", log_match.group("method"))
            return

        log_match = re.search(r"^\[(?P<tag>[A-Z].*?)\]", line)
        if log_match is not None:
            self._logger.info(line)
            return


    def process_stderr_line(self, line: str) -> None:
        if line.startswith("debugger-agent: Unable to listen on "):
            return

        self._logger.error(line.rstrip())


    def process_stdout_end(self) -> None:
        pass


    def process_stderr_end(self) -> None:
        pass


    def _convert_shader_type_to_full_name(self, shader_type: str) -> str:
        if shader_type == "vp":
            return "Vertex Shader"
        if shader_type == "fp":
            return "Fragment Shader"

        raise ValueError("Unsupported shader type: '%s'" % shader_type)
