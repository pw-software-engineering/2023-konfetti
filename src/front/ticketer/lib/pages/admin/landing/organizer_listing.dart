import 'dart:developer';

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

  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Organizer> _organizers = [];
  
  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getHeader(),
          _getOrganizersList()
        ],
      ),
    );
  }

  Widget _getOrganizersList() {
    return Column(
      children: [
        ListView.builder(
          shrinkWrap: true,
          itemBuilder: (_, index) {
            if (index < _organizers.length) {
              return _getOrganizerListItem(_organizers[index]);
            } else if (_hasNextPage) {
              try {
                _fetchMoreData();
                setState(() {
                  _pageNo++;
                });
              } catch (e) {
                log(e.toString());
              }
              return const SizedBox(
                width: 50,
                height: 50,
                child: Center(
                  child: CircularProgressIndicator(),
                ),
              );
            }
            else {
              return Container(
                  margin: const EdgeInsets.only(top: 15.0),
                  child: const Center(
                      child: Text(
                        "No new applications to show!",
                        style: TextStyle(fontSize: 15, color: Colors.blue),
                      ))
              );
            }
          },
          itemCount: _hasNextPage ? _organizers.length + 1 : (_organizers.isEmpty ? 1: _organizers.length),
        ),
      ],
    );
  }

  Future<void> _fetchMoreData() async {
    final res = await OrganizerCommunication().listToVerify(_pageNo, _pageSize);
    setState(() {
      int before = _organizers.length;
      for (var org in res.item1.data["items"]) {
        _organizers.add(Organizer.fromJson(org));
      }
      int after = _organizers.length;
      _hasNextPage = after != 0 && before != after;
    });
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
        _rejectButton(organizer),
        _verifyButton(organizer)
      ],
    );
  }

  Widget _verifyButton(Organizer organizer) {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => decideOrganizer(organizer, true),
        child: const Text("Approve"),
      ),
    );
  }

  Widget _rejectButton(Organizer organizer) {
    return Container(
      margin: const EdgeInsets.all(15.0),
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.redAccent,
          fixedSize: const Size(90, 30),
        ),
        onPressed: () => decideOrganizer(organizer, false),
        child: const Text("Reject"),
      ),
    );
  }

  void decideOrganizer(Organizer organizer, bool isAccepted) async {
    var response = await OrganizerCommunication().decide(organizer, isAccepted);
    if (response.item2 == ResponseCode.allGood) {
      setState(() {
        _organizers.remove(organizer);
      });
    }
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