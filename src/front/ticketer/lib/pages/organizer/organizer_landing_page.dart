import 'package:flutter/material.dart';
import 'package:ticketer/pages/common/app_bar.dart';

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
          Container(
            padding: const EdgeInsets.all(9.0),
            margin: const EdgeInsets.only(top: 20),
            decoration:
                BoxDecoration(border: Border.all(color: Colors.blueGrey)),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  _getOrganizerInfo("Company name", "Januszex PL"),
                  _getOrganizerInfo("Address", "ul. Kwiatowa 7, 43-400 Poznań"),
                  _getOrganizerInfo("Display name", "Twoje imprezy u Janusza"),
                  _getOrganizerInfo("Tax info", "KRS: 0412941203942"),
                  _getOrganizerInfo("Phone", "+48 601 421 449")
                ],
              ),
            ),
          )
        ],
      ),
    );
  }

  Widget _getOrganizerInfo(String property, String value) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Flexible(
          flex: 2,
          child: Text(
            property,
            style: const TextStyle(
                fontWeight: FontWeight.w700, color: Colors.blue, fontSize: 16),
          ),
        ),
        Flexible(
          flex: 5,
          child: Text(
            value,
            style: const TextStyle(fontSize: 16),
          ),
        ),
      ],
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
