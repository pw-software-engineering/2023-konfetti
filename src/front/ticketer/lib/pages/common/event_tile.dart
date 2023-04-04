import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/model/event.dart';

class EventTile extends StatefulWidget {
  final Event event;

  const EventTile({Key? key, required this.event}) : super(key: key);

  @override
  State<EventTile> createState() => _EventTileState();
}

class _EventTileState extends State<EventTile> {
  late Event _event;

  Widget _eventContainer() {
    return InkWell(
      onTap: () => _showEventDetails(),
      child: Container(
        padding: const EdgeInsets.all(9.0),
        decoration: BoxDecoration(border: Border.all(color: Colors.blueGrey)),
        child: SingleChildScrollView(
          child: Column(
            children: [
              _getEventInfo("Event name", _event.name,
                  style: const TextStyle(fontWeight: FontWeight.bold)),
              _getEventInfo("Date & time", _event.date),
              _getEventInfo("Location", _event.location),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _showEventDetails() async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(_event.name),
          scrollable: true,
          content: SingleChildScrollView(
            child: SizedBox(
              width: MediaQuery.of(context).size.width / 4,
              height: MediaQuery.of(context).size.height / 2,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    _event.description,
                    textAlign: TextAlign.justify,
                  ),
                  const Text(
                    "\nSectors: ",
                    style: TextStyle(
                      fontWeight: FontWeight.w600,
                      color: Colors.blueAccent,
                    ),
                  ),
                  SingleChildScrollView(
                    child: ListView(
                      shrinkWrap: true,
                      children: _event.sectors
                          .map((s) => Text(s.toString()))
                          .toList(),
                    ),
                  )
                ],
              ),
            ),
          ),
          actions: [
            ElevatedButton(
              onPressed: () => {
                Navigator.pop(context),
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  Widget _getEventInfo(String property, String value, {TextStyle? style}) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Flexible(
          flex: 2,
          child: Text(
            property,
            style: TextStyle(
                fontWeight: FontWeight.w700,
                color: Theme.of(context).primaryColor,
                fontSize: 16),
          ),
        ),
        Flexible(
          flex: 5,
          child: Text(
            value,
            style: TextStyle(fontSize: 16, fontWeight: style?.fontWeight),
          ),
        ),
      ],
    );
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Center(
        child: _eventContainer(),
      ),
    );
  }

  @override
  void initState() {
    _event = widget.event;
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
