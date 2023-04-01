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
          _organizerListItem(),
          _organizerListItem(),
          _organizerListItem(),
        ],
      ),
    );
  }

  Widget _organizerListItem() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        const OrganizerCard(),
        _verifyButton(),
        _rejectButton()
      ],
    );
  }

  Widget _verifyButton() {
    return Container(
      margin: const EdgeInsets.only(top: 15.0),
      child: ElevatedButton(
        onPressed: () => {},
        child: Text("Verify"),
      ),
    );
  }

  Widget _rejectButton() {
    return Container(
      margin: const EdgeInsets.only(top: 15.0),
      child: ElevatedButton(
        onPressed: () => {},
        child: Text("Reject"),
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