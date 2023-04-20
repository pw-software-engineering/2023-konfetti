import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/event_tile.dart';
import 'package:ticketer/pages/organizer/organizer_drawer.dart';

class OrganizerEventListPage extends StatefulWidget {
  const OrganizerEventListPage({Key? key}) : super(key: key);

  @override
  State<OrganizerEventListPage> createState() => _OrganizerEventListPageState();
}

class _OrganizerEventListPageState extends State<OrganizerEventListPage> {
  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Event> _events = [];

  Future<void> _fetchMoreData() async {
    final res =
        await BackendCommunication().event.organizerMyList(_pageNo, _pageSize);
    setState(() {
      int before = _events.length;
      for (var ev in res.item1.data["items"]) {
        _events.add(Event.fromJson(ev));
      }
      int after = _events.length;
      _hasNextPage = before != after;
    });
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getHeader(),
          _getEventsList(),
        ],
      ),
    );
  }

  Container _getHeader() {
    return Container(
      margin: const EdgeInsets.only(top: 30.0),
      child: Text(
        "Your events",
        style: TextStyle(
          fontSize: 22,
          fontWeight: FontWeight.w600,
          color: Theme.of(context).primaryColor,
        ),
      ),
    );
  }

  Widget _getEventsList() {
    return Column(
      children: [
        ListView.builder(
          shrinkWrap: true,
          itemBuilder: (_, index) {
            if (_events.isEmpty && !_hasNextPage) {
              return Container(
                margin: const EdgeInsets.only(top: 15.0),
                child: const Center(
                  child: Text(
                    "No events created yet",
                    style: TextStyle(fontSize: 15),
                  ),
                ),
              );
            } else if (index < _events.length) {
              return EventTile(event: _events[index]);
            } else {
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
          },
          itemCount: _hasNextPage ? _events.length + 1 : _events.length,
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: ticketerAppBar("My events"),
      drawer: const OrganizerNavigationDrawer(),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
          padding: const EdgeInsets.all(20),
          margin: const EdgeInsets.all(8),
          child: _getContent(),
        ),
      ),
    );
  }
}
