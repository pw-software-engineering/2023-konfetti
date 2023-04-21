import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';

class OrganizerCard extends StatefulWidget {
  final Organizer organizer;

  const OrganizerCard({Key? key, required this.organizer}) : super(key: key);

  @override
  State<OrganizerCard> createState() => _OrganizerCardState();
}

class _OrganizerCardState extends State<OrganizerCard> {
  late Organizer organizer;

  @override
  void initState() {
    organizer = widget.organizer;
    super.initState();
  }

  Container _organizerContainer() {
    return Container(
      padding: const EdgeInsets.all(9.0),
      decoration: BoxDecoration(
          border: Border.all(
        color: Theme.of(context).hintColor,
      )),
      child: SingleChildScrollView(
        child: Column(
          children: [
            _getOrganizerInfo("Company name", organizer.companyName),
            _getOrganizerInfo("Address", organizer.address),
            _getOrganizerInfo("Display name", organizer.displayName),
            _getOrganizerInfo("Tax info", "${organizer.taxIdType.name}: ${organizer.taxId}"),
            _getOrganizerInfo("Email", organizer.email),
            _getOrganizerInfo("Phone", organizer.phoneNumber)
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
            style: TextStyle(
                fontWeight: FontWeight.w700,
                color: Theme.of(context).primaryColor,
                fontSize: 16),
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
        children: [_organizerContainer()],
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
