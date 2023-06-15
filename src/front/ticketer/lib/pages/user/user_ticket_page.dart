import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/ticket.dart';

class UserTicketPage extends StatefulWidget {
  const UserTicketPage({Key? key}) : super(key: key);

  @override
  State<UserTicketPage> createState() => _UserTicketPageState();
}

class _UserTicketPageState extends State<UserTicketPage> {
  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Ticket> _tickets = [];

  Future<void> _fetchMoreData() async {
    final res = await BackendCommunication().ticket.list(_pageNo, _pageSize);
    setState(() {
      int before = _tickets.length;
      for (var ev in res.item1.data["items"]) {
        _tickets.add(Ticket.fromJson(ev));
        log(_tickets.toString());
      }
      int after = _tickets.length;
      _hasNextPage = before != after;
    });
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getGreeting(),
          _getTicketList(),
        ],
      ),
    );
  }

  Text _getGreeting() {
    return Text(
      "Your tickets",
      style: TextStyle(
        fontSize: 26,
        color: Theme.of(context).primaryColor,
      ),
    );
  }

  Widget _getTicketList() {
    return Column(
      children: [
        ListView.builder(
          shrinkWrap: true,
          itemBuilder: (_, index) {
            if (_tickets.isEmpty || !_hasNextPage) {
              return Container(
                margin: const EdgeInsets.only(top: 15.0),
                child: const Center(
                  child: Text(
                    "No events available",
                    style: TextStyle(fontSize: 15),
                  ),
                ),
              );
            } else if (index < _tickets.length) {
              return Text(_tickets[index].event.name);
            } else if (_hasNextPage) {
              try {
                _fetchMoreData();
                Future.delayed(
                  Duration.zero,
                  () {
                    setState(
                      () {
                        _pageNo++;
                      },
                    );
                  },
                );
              } catch (e) {
                log(e.toString());
              }
            } else {
              return const SizedBox(
                width: 50,
                height: 50,
                child: Center(
                  child: CircularProgressIndicator(),
                ),
              );
            }
          },
          itemCount: _hasNextPage ? _tickets.length + 1 : _tickets.length,
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("My events"),
      ),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 400),
          child: _getContent(),
        ),
      ),
    );
  }
}
