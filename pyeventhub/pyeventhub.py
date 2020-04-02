"""
 pyeventhub - a CLI that sends messages to an Azure Event Hub.
"""

import asyncio
import random
import time
import json
from itertools import count
from datetime import datetime, timedelta
from argparse import ArgumentParser
from azure.eventhub.aio import EventHubProducerClient
from azure.eventhub import EventData


def _create_parser():
    """
        Creates the argument parser.

        Returns:
            The argument parser.
    """
    parser = ArgumentParser(description="A CLI that sends messages to an Azure event hub.")

    parser.add_argument("--connection-string", type=str, required=True,
                        help="The Azure event hub connection string")

    parser.add_argument("--name", type=str, required=True,
                        help="The Azure event hub name")

    parser.add_argument("--interval", type=int, required=False,
                        help="The number of seconds to wait between sends. Defaults to 10 seconds.")

    parser.add_argument("--what-if", type=bool, required=False,
                        help="Run the program without sending messages to the Event Hub. "
                             "The app will log what would have been sent to the Event Hub.")

    return parser


def _create_event_data(index):
    """
        Creates event data that is sent to the event hub.

        Args:
            The data's index which is used as the "name" property of the event hub data.

        Returns:
            A dictionary containing the event hub data.
    """
    time_stamp = str(datetime.utcnow() + timedelta(seconds=index))
    name = str(index)
    metric = random.randint(0, 1000)

    return {"timeStamp": time_stamp, "name": name, "metric": metric, "source": "pyeventhub"}


async def _send_message(producer, event_data):
    """
        Sends a message to the event hub.

        Args:
            producer: The EventHubProducerClient.
            event_data: A dictionary containing the event data to send.
    """
    batch = await producer.create_batch()
    batch.add(EventData(_serialize_event_data_as_json(event_data)))
    await producer.send_batch(batch)


def _serialize_event_data_as_json(event_data):
    """
        Serializes event data to a JSON string.

        Args:
            event_data: The event data dictionary to serialize.

        Returns:
            The event data as a JSON string.
    """
    return json.dumps(event_data)


def _print_send_status(event_data):
    """
        Prints a status after a message has been sent.

        Args:
            event_data: A dictionary containing the event data that was sent.
    """
    message_count = (int(event_data["name"]) - 1000) + 1

    if message_count % 5 == 0:
        print(f"Sent {message_count} messages.", end="\r")


async def _run(params):
    """
        Runs the application.

        Args:
            params: A dictionary containing the following:
                connection_string: The event hub namespace connection string.
                name: The event hub name.
                interval: The number of seconds to wait between message sends.
                what_if: When true does not send the message to the event hub.
                    Instead it prints what would have been sent.
    """

    producer = EventHubProducerClient.from_connection_string(
        params["connection_string"],
        eventhub_name=params["name"])

    async with producer:
        for index in count(1000):
            event_data = _create_event_data(index)

            if params["what_if"]:
                print(event_data)
            else:
                await _send_message(
                    producer,
                    event_data)

                _print_send_status(event_data)

            time.sleep(params["interval"])


def _main():
    """
    The entry point of the application.
    """
    parser = _create_parser()
    args = parser.parse_args()

    if args.interval is None:
        args.interval = 10

    if args.what_if is None:
        args.what_if = False

    loop = asyncio.get_event_loop()

    params = {
        "connection_string": args.connection_string,
        "name": args.name,
        "interval": args.interval,
        "what_if": args.what_if
    }

    loop.run_until_complete(_run(params))


if __name__ == "__main__":
    _main()
