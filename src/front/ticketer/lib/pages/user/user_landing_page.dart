import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/event_tile.dart';
import 'package:ticketer/pages/user/user_drawer.dart';

class UserLandingPage extends StatefulWidget {
  const UserLandingPage({Key? key}) : super(key: key);

  @override
  State<UserLandingPage> createState() => _UserLandingPageState();
}

class _UserLandingPageState extends State<UserLandingPage> {
  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Event> _events = [];

  Future<void> _fetchMoreData() async {
    final res = await BackendCommunication().event.list(_pageNo, _pageSize);
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
          _getUserIcon(),
          _getGreeting(),
          _getEventsList(),
        ],
      ),
    );
  }

  Text _getGreeting() {
    return const Text(
      "Hello user",
      style: TextStyle(fontSize: 26, color: Colors.blue),
    );
  }

  Container _getUserIcon() {
    return Container(
      height: 60,
      width: 60,
      decoration: const BoxDecoration(
        shape: BoxShape.circle,
        color: Colors.blue,
      ),
      alignment: Alignment.center,
      child: const Icon(
        Icons.person,
        color: Colors.white,
        size: 40,
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
      appBar: ticketerAppBar("Welcome"),
      drawer: const UserNavigationDrawer(),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
          padding: const EdgeInsets.all(20),
          child: _getContent(),
        ),
      ),
    );
  }
}
