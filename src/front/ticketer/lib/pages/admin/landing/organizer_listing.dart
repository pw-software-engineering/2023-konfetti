import 'package:flutter/material.dart';
import 'package:ticketer/pages/common/organizer_card.dart';

class OrganizerListing extends StatefulWidget {
  const OrganizerListing({Key? key}) : super(key: key);

  @override
  State<OrganizerListing> createState() => _OrganizerListingPageState();

}

class _OrganizerListingPageState  extends State<OrganizerListing>  {

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getHeader(),
          _getOrganizerListItem(),
          _getOrganizerListItem(),
          _getOrganizerListItem(),
        ],
      ),
    );
  }

  Container _getHeader() {
    return Container(
      margin: const EdgeInsets.only(top: 30.0),
      child: const Text(
        "Organizer accounts waiting for approval",
        style: TextStyle(fontSize: 20, color: Colors.blue),
      ),
    );
  }

  Widget _getOrganizerListItem() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        const OrganizerCard(),
        _rejectButton(),
        _verifyButton()
      ],
    );
  }

  Widget _verifyButton() {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => {},
        child: const Text("Approve"),
      ),
    );
  }

  Widget _rejectButton() {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.redAccent,
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => {},
        child: const Text("Reject"),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Container(
        child: _getContent(),
      ),
    );
  }
}