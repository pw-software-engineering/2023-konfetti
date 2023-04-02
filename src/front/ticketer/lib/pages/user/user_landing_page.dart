import 'dart:convert';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/user/user_drawer.dart';

class UserLandingPage extends StatefulWidget {
  const UserLandingPage({Key? key}) : super(key: key);

  @override
  State<UserLandingPage> createState() => _UserLandingPageState();
}

class _UserLandingPageState extends State<UserLandingPage> {
  int _pageNo = 0;
  final int _pageSize = 20;
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
        Map<String, dynamic> parsedJson = res.item1.data;
        print(parsedJson);
        _events = parsedJson["items"].map((ev) => Event.fromJson(ev)).toList();
      });
    } catch (e) {
      log(e.toString());
    }

    setState(() {
      _isFirstLoadRunning = false;
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

  // Widget _getEventsList() {
  //   // return Column(
  //   //           children: [
  //   //             Expanded(
  //   //               child: ListView.builder(
  //   //                 controller: _controller,
  //   //                 itemCount: _posts.length,
  //   //                 itemBuilder: (_, index) => Card(
  //   //                   margin: const EdgeInsets.symmetric(
  //   //                       vertical: 8, horizontal: 10),
  //   //                   child: ListTile(
  //   //                     title: Text(_posts[index]['title']),
  //   //                     subtitle: Text(_posts[index]['body']),
  //   //                   ),
  //   //                 ),
  //   //               ),
  //   //             ),
  // }

  @override
  void initState() {
    super.initState();
    _firstLoad();
    //_controller = ScrollController()..addListener(_loadMore);
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
