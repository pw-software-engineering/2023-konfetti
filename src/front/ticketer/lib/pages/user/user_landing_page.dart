import 'dart:convert';
import 'dart:developer';
import 'dart:ui';

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
  final int _pageSize = 5;
  bool _hasNextPage = true;
  bool _isFirstLoadRunning = false;
  bool _isLoadMoreRunning = false;
  List<Event> _events = [];
  late ScrollController _controller;

  void _firstLoad() async {
    setState(() {
      _isFirstLoadRunning = true;
    });
    try {
      final res = await BackendCommunication().event.list(_pageNo, _pageSize);
      setState(() {
        _events = [];
        for (var ev in res.item1.data["items"]) {
          _events.add(Event.fromJson(ev));
        }
      });
    } catch (e) {
      log(e.toString());
    }

    setState(() {
      _isFirstLoadRunning = false;
    });
  }

  void _loadMore() async {
    print("Load more running");
    if (_hasNextPage == true &&
        _isFirstLoadRunning == false &&
        _isLoadMoreRunning == false &&
        _controller.position.extentAfter < 300) {
      setState(() {
        _isLoadMoreRunning = true; // Display a progress indicator at the bottom
        _pageNo += 1; // Increase _page by 1
      });
      try {
        final res = await BackendCommunication().event.list(_pageNo, _pageSize);

        final List fetchedEvents = res.item1.data["items"];
        if (fetchedEvents.isNotEmpty) {
          setState(() {
            for (var ev in res.item1.data["items"]) {
              _events.add(Event.fromJson(ev));
            }
          });
        } else {
          setState(() {
            _hasNextPage = false;
          });
        }
      } catch (e) {}

      setState(() {
        _isLoadMoreRunning = false;
      });
    }
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
    return ScrollConfiguration(
      behavior: ScrollConfiguration.of(context).copyWith(
        dragDevices: {
          PointerDeviceKind.touch,
          PointerDeviceKind.mouse,
          PointerDeviceKind.stylus,
          PointerDeviceKind.unknown,
        },
      ),
      child: Column(
        children: [
          ListView.builder(
            shrinkWrap: true,
            physics: const AlwaysScrollableScrollPhysics(),
            scrollDirection: Axis.vertical,
            controller: _controller,
            itemCount: _events.length,
            itemBuilder: (_, index) => EventTile(event: _events[index]),
          ),

          // when the _loadMore function is running
          if (_isLoadMoreRunning == true)
            const Padding(
              padding: EdgeInsets.only(top: 10, bottom: 40),
              child: Center(
                child: CircularProgressIndicator(),
              ),
            ),

          // When nothing else to load
          if (_hasNextPage == false)
            Container(
              padding: const EdgeInsets.only(top: 30, bottom: 40),
              color: Colors.amber,
              child: const Center(
                child: Text('You have fetched all of the content'),
              ),
            ),
        ],
      ),
    );
  }

  @override
  void initState() {
    _firstLoad();
    _controller = ScrollController();
    _controller.addListener(_loadMore);
    super.initState();
  }

  @override
  void dispose() {
    _controller.removeListener(_loadMore);
    super.dispose();
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
