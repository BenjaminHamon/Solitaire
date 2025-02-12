import logging
import os
from typing import TextIO


all_log_levels = [ "debug", "info", "warning", "error", "critical" ]
date_format_iso = "%Y-%m-%dT%H:%M:%S"


def get_level_as_integer(level_as_string: str) -> int:
    if level_as_string.lower() == "debug":
        return logging.DEBUG
    if level_as_string.lower() == "info":
        return logging.INFO
    if level_as_string.lower() == "warning":
        return logging.WARNING
    if level_as_string.lower() == "error":
        return logging.ERROR
    if level_as_string.lower() == "critical":
        return logging.CRITICAL

    raise ValueError("Unknown logging level '%s'" % level_as_string)


def configure_log_stream(logger: logging.Logger, stream: TextIO, level: str, message_format: str, date_format: str) -> None:
    formatter = logging.Formatter(fmt = message_format, datefmt = date_format, style = "{")

    stream_handler = logging.StreamHandler(stream)
    stream_handler.setLevel(get_level_as_integer(level))
    stream_handler.formatter = formatter

    logger.addHandler(stream_handler)


def configure_log_file( # pylint: disable = too-many-arguments
        logger: logging.Logger, file_path: str, level: str, message_format: str, date_format: str, mode: str, encoding: str) -> None:

    if os.path.dirname(file_path):
        os.makedirs(os.path.dirname(file_path), exist_ok = True)

    formatter = logging.Formatter(fmt = message_format, datefmt = date_format, style = "{")

    file_handler = logging.FileHandler(file_path, mode = mode, encoding = encoding)
    file_handler.setLevel(get_level_as_integer(level))
    file_handler.formatter = formatter

    logger.addHandler(file_handler)
