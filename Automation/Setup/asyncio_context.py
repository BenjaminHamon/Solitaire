# cspell:words sigbreak

import asyncio
import platform
import signal
from typing import Any, List, Tuple


class AsyncioContext:
    """ Wrapper around asyncio.run to offer graceful termination """


    def __init__(self) -> None:
        self.should_shutdown = False
        self.shutdown_request_counter = 0
        self.shutdown_request_counter_limit = 3
        self.shutdown_timeout_seconds = 30


    def run(self, coroutine: Any) -> None:
        system = platform.system()

        old_signal_handlers: List[Tuple[signal.Signals,signal._HANDLER]] = [] # pylint: disable = no-member

        if system == "Windows":
            old_sigbreak_handler = signal.signal(signal.SIGBREAK, lambda signal_number, frame: self.shutdown()) # pylint: disable = no-member
            old_signal_handlers.append((signal.SIGBREAK, old_sigbreak_handler))

        old_sigint_handler = signal.signal(signal.SIGINT, lambda signal_number, frame: self.shutdown())
        old_signal_handlers.append((signal.SIGINT, old_sigint_handler))
        old_sigterm_handler = signal.signal(signal.SIGTERM, lambda signal_number, frame: self.shutdown())
        old_signal_handlers.append((signal.SIGTERM, old_sigterm_handler))

        try:
            asyncio.run(self.run_async(coroutine))
        finally:
            for signal_value, signal_handler in old_signal_handlers:
                signal.signal(signal_value, signal_handler)


    async def run_async(self, coroutine: Any) -> None:
        future = asyncio.ensure_future(coroutine)

        try:
            while not self.should_shutdown and not future.done():
                await asyncio.sleep(1)

            if self.should_shutdown:
                raise RuntimeError("Async operation was interrupted")

        finally:
            if not future.done():
                future.cancel()

            try:
                await asyncio.wait_for(future, timeout = self.shutdown_timeout_seconds)
            except asyncio.CancelledError:
                pass

            if not future.done():
                raise RuntimeError("Future did not complete")


    def shutdown(self) -> None:
        self.should_shutdown = True

        self.shutdown_request_counter += 1
        if self.shutdown_request_counter > self.shutdown_request_counter_limit:
            raise RuntimeError("Forcing shutdown (many requests)")
