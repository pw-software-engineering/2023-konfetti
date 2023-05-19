import 'package:flutter/material.dart';
import 'package:ticketer/pages/admin/admin_drawer.dart';
import 'package:ticketer/pages/admin/events/event_listing.dart';
import 'package:ticketer/pages/common/app_bar.dart';

class AdminVerifyEventsPage extends StatefulWidget {
  const AdminVerifyEventsPage({Key? key}) : super(key: key);

  @override
  State<AdminVerifyEventsPage> createState() => _AdminVerifyEventsPageState();
}

class _AdminVerifyEventsPageState extends State<AdminVerifyEventsPage> {
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [_getUserIcon(), _getGreeting(), const EventListing()],
      ),
    );
  }

  Text _getGreeting() {
    return Text(
      "Hello admin",
      style: TextStyle(fontSize: 26, color: Theme.of(context).primaryColor),
    );
  }

  Container _getUserIcon() {
    return Container(
      height: 60,
      width: 60,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        color: Theme.of(context).primaryColor,
      ),
      alignment: Alignment.center,
      child: const Icon(
        Icons.person,
        color: Colors.white,
        size: 40,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: ticketerAppBar("Welcome"),
      drawer: const AdminNavigationDrawer(),
      body: Center(
        child: Container(
          child: _getContent(),
        ),
      ),
    );
  }
}
