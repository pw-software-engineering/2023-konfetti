import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/event/communication_event.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/pages/common/event_tile.dart';

class EventListing extends StatefulWidget {
  const EventListing({Key? key}) : super(key: key);

  @override
  State<EventListing> createState() => _EventListingState();
}

class _EventListingState  extends State<EventListing>  {

  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Event> _events = [];

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getHeader(),
          _getEventsList()
        ],
      ),
    );
  }

  Widget _getEventsList() {
    return Column(
      children: [
        ListView.builder(
          shrinkWrap: true,
          itemBuilder: (_, index) {
            if (index < _events.length) {
              return _getEventListItem(_events[index]);
            } else if (_hasNextPage) {
              try {
                _fetchMoreData();
                setState(() {
                  _pageNo++;
                });
              } catch (e) {
                log(e.toString());
              }
              return const SizedBox(
                width: 50,
                height: 50,
                child: Center(
                  child: CircularProgressIndicator(),
                ),
              );
            }
            else {
              return Container(
                  margin: const EdgeInsets.only(top: 15.0),
                  child: const Center(
                      child: Text(
                        "No new events to show!",
                        style: TextStyle(fontSize: 15, color: Colors.blue),
                      ))
              );
            }
          },
          itemCount: _hasNextPage ? _events.length + 1 : (_events.isEmpty ? 1: _events.length),
        ),
      ],
    );
  }

  Future<void> _fetchMoreData() async {
    final res = await EventCommunication().listToVerify(_pageNo, _pageSize);
    setState(() {
      int before = _events.length;
      for (var org in res.item1.data["items"]) {
        _events.add(Event.fromJson(org));
      }
      int after = _events.length;
      _hasNextPage = after != 0 && before != after;
    });
  }

  Container _getHeader() {
    return Container(
      margin: const EdgeInsets.only(top: 30.0),
      child: Text(
        "Events waiting for approval",
        style: TextStyle(fontSize: 20, color: Theme.of(context).primaryColor),
      ),
    );
  }

  Widget _getEventListItem(Event event) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        EventTile(event: event), _rejectButton(event), _verifyButton(event)],
    );
  }

  Widget _verifyButton(Event event) {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => decideEvent(event, true),
        child: const Text("Approve"),
      ),
    );
  }

  Widget _rejectButton(Event event) {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.redAccent,
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => decideEvent(event, false),
        child: const Text("Reject"),
      ),
    );
  }

  void decideEvent(Event event, bool isAccepted) async {
    var response = await EventCommunication().decide(event, isAccepted);
    if (response.item2 == ResponseCode.allGood) {
      setState(() {
        _events.remove(event);
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Container(
        child: _getContent(),
      ),
    );
  }
}
