import 'package:flutter/material.dart';

class OrganizerCard extends StatefulWidget {
  const OrganizerCard({Key? key}) : super(key: key);

  @override
  State<OrganizerCard> createState() => _OrganizerCardState();
}

class _OrganizerCardState extends State<OrganizerCard> {


  Container _organizerContainer() {
    return Container(
      padding: const EdgeInsets.all(9.0),
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

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _organizerContainer()
        ],
      ),
    );
  }


  @override
  Widget build(BuildContext context) {
    return Container(
      constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
      padding: const EdgeInsets.all(20),
      child: _getContent(),
    );
  }
}
