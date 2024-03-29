import 'package:flutter/material.dart';
import 'package:input_quantity/input_quantity.dart';
import 'package:ticketer/auth/auth.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/account_type.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/event_status.dart';
import 'package:ticketer/backend_communication/model/sector.dart';
import 'package:ticketer/pages/organizer/organizer_event_edit.dart';
import 'package:ticketer/pages/organizer/organizer_landing_page.dart';
import 'package:ticketer/pages/user/payment_page.dart';

class EventTile extends StatefulWidget {
  final Event event;

  const EventTile({Key? key, required this.event}) : super(key: key);

  @override
  State<EventTile> createState() => _EventTileState();
}

class _EventTileState extends State<EventTile> {
  late Event _event;
  late AccountType _accountType;
  late List<int> _seatsInSectors;

  Widget _eventContainer() {
    return InkWell(
      onTap: () => _showEventDetails(),
      child: Container(
        padding: const EdgeInsets.all(9.0),
        decoration: BoxDecoration(
          border: Border.all(
            color: Theme.of(context).hintColor,
          ),
        ),
        child: SingleChildScrollView(
          child: Column(
            children: [
              _getEventInfo("Event name", _event.name,
                  style: const TextStyle(fontWeight: FontWeight.bold)),
              _getEventInfo("Date & time", _event.date),
              _getEventInfo("Location", _event.location),
              _getEventInfo("Status", _event.status!.getStatusName()),
            ],
          ),
        ),
      ),
    );
  }

  Future<void> _showEventDetails() async {
    await showDialog(
      context: context,
      builder: (context) {
        return _showDetails(context);
      },
    );
  }

  AlertDialog _showDetails(BuildContext context) {
    return AlertDialog(
      title: Text(
        _event.name,
        style: const TextStyle(
          fontSize: 20,
          fontWeight: FontWeight.w600,
        ),
      ),
      scrollable: true,
      content: SingleChildScrollView(
        child: SizedBox(
          width: MediaQuery.of(context).size.width / 4,
          height: MediaQuery.of(context).size.height / 2,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _getEventInfo("Event name", _event.name,
                  style: const TextStyle(fontWeight: FontWeight.bold)),
              _getEventInfo("Date & time", _event.date),
              _getEventInfo("Location", _event.location),
              _getEventInfo("Status", _event.status!.getStatusName()),
              Text(
                _event.description,
                textAlign: TextAlign.justify,
              ),
              Text(
                "\nSectors: ",
                style: TextStyle(
                  fontWeight: FontWeight.w600,
                  color: Theme.of(context).primaryColorDark,
                  fontSize: 18,
                ),
              ),
              _accountType == AccountType.User && _event.status == EventStatus.Opened
                  ? _getPurchasableSectorList()
                  : _getSimpleSectorList(),
            ],
          ),
        ),
      ),
      actions: _accountType == AccountType.User
          ? _event.status == EventStatus.Opened
              ? _getUserBuyActions()
              : _getSimpleActions()
          : _getOrganizerActions(),
    );
  }

  List<Widget> _getSimpleActions() {
    return [
      ElevatedButton(
        onPressed: () => {
          Navigator.pop(context),
        },
        child: const Text('OK'),
      ),
    ];
  }

  List<Widget> _getOrganizerActions() {
    return [
      Visibility(
          visible: _event.status != EventStatus.Unverified,
          child: _getEventStateButton()),
      ElevatedButton(
        onPressed: () => {_navigateToEdit()},
        child: const Text('Edit'),
      ),
      ElevatedButton(
        onPressed: () => {
          Navigator.pop(context),
        },
        child: const Text('OK'),
      ),
    ];
  }

  Widget _getEventStateButton() {
    return _event.status == EventStatus.Verified
        ? _getPublishButton()
        : (_event.status == EventStatus.Published ||
                _event.status == EventStatus.Closed
            ? _getStartSaleButton()
            : (_event.status == EventStatus.Opened
                ? _getStopSaleButton()
                : ElevatedButton(onPressed: () => {}, child: const Text(""))));
  }

  Widget _getPublishButton() {
    return ElevatedButton(
      onPressed: () async => {
        await BackendCommunication().event.publish(_event),
        Navigator.pushReplacement(
            context,
            MaterialPageRoute(
                builder: (context) => const OrganizerLandingPage()))
      },
      child: const Text("Publish"),
    );
  }

  Widget _getStartSaleButton() {
    return ElevatedButton(
      onPressed: () async => {
        await BackendCommunication().event.saleStart(_event),
        Navigator.pushReplacement(
            context,
            MaterialPageRoute(
                builder: (context) => const OrganizerLandingPage()))
      },
      child: const Text("Start sale"),
    );
  }

  Widget _getStopSaleButton() {
    return ElevatedButton(
      onPressed: () async => {
        await BackendCommunication().event.saleStop(_event),
        Navigator.pushReplacement(
            context,
            MaterialPageRoute(
                builder: (context) => const OrganizerLandingPage()))
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: Colors.redAccent,
      ),
      child: const Text("Stop sale"),
    );
  }

  void _navigateToEdit() {
    Navigator.pop(context);
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: ((context) => EventEditPage(event: _event)),
      ),
    );
  }

  List<Widget> _getUserBuyActions() {
    return [
      ElevatedButton(
        onPressed: () => {
          Navigator.pop(context),
        },
        style: ElevatedButton.styleFrom(
          backgroundColor: Colors.redAccent,
          fixedSize: const Size(100, 35),
        ),
        child: const Text('Cancel'),
      ),
      Visibility(
        visible: _event.status == EventStatus.Opened,
        child: ElevatedButton(
          onPressed: () => _navigateToPayment(),
          style: ElevatedButton.styleFrom(
            fixedSize: const Size(100, 35),
          ),
          child: const Text('Purchase'),
        ),
      ),
    ];
  }

  void _navigateToPayment() {
    if (!_seatsInSectors.any((element) => element > 0)) return;
    Navigator.pop(context);
    Navigator.push(
      context,
      MaterialPageRoute(
        builder: ((context) =>
            PaymentPage(event: _event, seatsInSectors: _seatsInSectors)),
      ),
    );
  }

  SingleChildScrollView _getSimpleSectorList() {
    return SingleChildScrollView(
      child: ListView(
        shrinkWrap: true,
        children: _event.sectors.map((s) => Text(s.toString())).toList(),
      ),
    );
  }

  SingleChildScrollView _getPurchasableSectorList() {
    return SingleChildScrollView(
      child: ListView(
        shrinkWrap: true,
        children: _event.sectors.asMap().entries.map((s) {
          int idx = s.key;
          var val = s.value;
          return _getSectorPicker(val, idx);
        }).toList(),
      ),
    );
  }

  Widget _getSectorPicker(Sector s, int index) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Flexible(
          flex: 3,
          child: Text(s.toString()),
        ),
        Flexible(
          flex: 2,
          child: InputQty(
            btnColor1: Theme.of(context).primaryColor,
            minVal: 0,
            initVal: 0,
            showMessageLimit: false,
            boxDecoration: const BoxDecoration(),
            isIntrinsicWidth: false,
            textFieldDecoration: const InputDecoration(
              isDense: false,
              contentPadding: EdgeInsets.symmetric(horizontal: 5),
            ),
            onQtyChanged: (v) {
              Future.delayed(
                Duration.zero,
                () {
                  setState(
                    () {
                      _seatsInSectors[index] = v!.toInt();
                    },
                  );
                },
              );
            },
          ),
        ),
      ],
    );
  }

  Widget _getEventInfo(String property, String value, {TextStyle? style}) {
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
            style: TextStyle(fontSize: 16, fontWeight: style?.fontWeight),
          ),
        ),
      ],
    );
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Center(
        child: _eventContainer(),
      ),
    );
  }

  @override
  void initState() {
    _event = widget.event;
    _seatsInSectors = List<int>.filled(_event.sectors.length, 0);
    _accountType = Auth().getCurrentAccount!.type;
    super.initState();
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
