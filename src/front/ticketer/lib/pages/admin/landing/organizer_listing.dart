import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/organizer/communication_organizer.dart';
import 'package:ticketer/backend_communication/model/organizer.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/pages/common/organizer_card.dart';

class OrganizerListing extends StatefulWidget {
  const OrganizerListing({Key? key}) : super(key: key);

  @override
  State<OrganizerListing> createState() => _OrganizerListingPageState();

}

class _OrganizerListingPageState  extends State<OrganizerListing>  {
  
  List<Organizer> data = [];
  bool end = false;
  int currentPageNumber = 0;
  static const int pageSize = 1;
  
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getHeader(),
          _getFlatList()
        ],
      ),
    );
  }

  ListView _getFlatList() {
    return ListView.builder(
      shrinkWrap: true,
      itemBuilder: (context, index) {
        if (index < data.length) {
          return _getOrganizerListItem(data[index]);
        } else if (index == data.length && end) {
          return const Center(child: Text('End of list'));
        } else {
          _getMoreData();
          return const SizedBox(
            height: 80,
            child: Center(child: CircularProgressIndicator()),
          );
        }
      },
      itemCount: data.length + 1,
    );
  }

  void _getMoreData() async {
    final response = await OrganizerCommunication().listToVerify(currentPageNumber, pageSize);
    if (!mounted) return;

    if (response.item2 != ResponseCode.allGood) {
      if (response.item1.data.items.isEmpty) {
        setState(() => end = true);
        return;
      }
    } else {
      setState(() => data = [...data, ...response.item1.data.items]);
      currentPageNumber += 1;
    }
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

  Widget _getOrganizerListItem(Organizer organizer) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        OrganizerCard(organizer: organizer),
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