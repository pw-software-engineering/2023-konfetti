import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/model/event.dart';

class EventTile extends StatefulWidget {
  final Event event;

  const EventTile({Key? key, required this.event}) : super(key: key);

  @override
  State<EventTile> createState() => _EventTileState();
}

class _EventTileState extends State<EventTile> {
  late Event event;

  Container _eventContainer() {
    return Container(
      padding: const EdgeInsets.all(9.0),
      decoration: BoxDecoration(border: Border.all(color: Colors.blueGrey)),
      child: SingleChildScrollView(
        child: Column(
          children: [
            _getEventInfo("Event name", event.name),
            _getEventInfo("Date & time", event.date),
            _getEventInfo("Location", event.location),
          ],
        ),
      ),
    );
  }

  Widget _getEventInfo(String property, String value) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Flexible(
          flex: 2,
          child: Text(
            property,
            style: const TextStyle(
                fontWeight: FontWeight.w700, color: Colors.blue, fontSize: 16),
          ),
        ),
        Flexible(
          flex: 5,
          child: Text(
            value,
            style: const TextStyle(fontSize: 16),
          ),
        ),
      ],
    );
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [_eventContainer()],
      ),
    );
  }

  @override
  void initState() {
    event = widget.event;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
      padding: const EdgeInsets.all(20),
      child: _getContent(),
    );
  }
}
