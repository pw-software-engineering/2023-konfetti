import 'package:flutter/material.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/organizer_card.dart';
import 'package:ticketer/pages/organizer/organizer_drawer.dart';

class OrganizerLandingPage extends StatefulWidget {
  const OrganizerLandingPage({Key? key}) : super(key: key);

  @override
  State<OrganizerLandingPage> createState() => _OrganizerLandingPageState();
}

class _OrganizerLandingPageState extends State<OrganizerLandingPage> {
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getUserIcon(),
          _getGreeting(),
          const OrganizerCard()
        ],
      ),
    );
  }

  Text _getGreeting() {
    return const Text(
      "Hello organizer",
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

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: ticketerAppBar("Welcome"),
      drawer: const OrganizerNavigationDrawer(),
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
