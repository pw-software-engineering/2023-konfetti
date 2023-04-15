import 'package:flutter/material.dart';
import 'package:ticketer/pages/admin/admin_drawer.dart';
import 'package:ticketer/pages/admin/landing/organizer_listing.dart';
import 'package:ticketer/pages/common/app_bar.dart';

class AdminLandingPage extends StatefulWidget {
  const AdminLandingPage({Key? key}) : super(key: key);

  @override
  State<AdminLandingPage> createState() => _AdminLandingPageState();
}

class _AdminLandingPageState extends State<AdminLandingPage> {
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [_getUserIcon(), _getGreeting(), const OrganizerListing()],
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
