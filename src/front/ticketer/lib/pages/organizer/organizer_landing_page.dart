import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/model/sector.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/organizer/organizer_drawer.dart';

class OrganizerLandingPage extends StatefulWidget {
  const OrganizerLandingPage({Key? key}) : super(key: key);

  @override
  State<OrganizerLandingPage> createState() => _OrganizerLandingPageState();
}

class _OrganizerLandingPageState extends State<OrganizerLandingPage> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _eventName = TextEditingController();
  final TextEditingController _eventDescription = TextEditingController();
  final TextEditingController _eventLocation = TextEditingController();
  final TextEditingController _eventDate = TextEditingController();
  final TextEditingController _sectorName = TextEditingController();
  final TextEditingController _sectorPrice = TextEditingController();
  final TextEditingController _sectorColumns = TextEditingController();
  final TextEditingController _sectorRows = TextEditingController();

  List<Sector> sectors = List.empty(growable: true);

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getUserIcon(),
          _getGreeting(),
          Container(
            padding: const EdgeInsets.all(12.0),
            margin: const EdgeInsets.only(top: 20, bottom: 20),
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
          ),
          _getEventCreationBanner(),
          Container(
              padding: const EdgeInsets.all(12.0),
              margin: const EdgeInsets.only(top: 20, bottom: 20),
              decoration:
                  BoxDecoration(border: Border.all(color: Colors.blueGrey)),
              child: Column(
                children: [
                  _getEventCreationForm(),
                ],
              )),
        ],
      ),
    );
  }

  Text _getEventCreationBanner() {
    return const Text(
      "Create new event",
      style: TextStyle(fontSize: 24, color: Colors.blue),
    );
  }

  Widget _getEventCreationForm() {
    return Form(
      key: _formKey,
      child: SingleChildScrollView(
        child: Column(
          children: [
            _eventNameEntryField(),
            _eventDescriptionEntryField(),
            _eventLocationEntryField(),
            _eventDateEntryField(),
            _getSectorEntryField(),
            _getSectorButtons(),
            _getSectorsList(),
            _submitButton(),
          ],
        ),
      ),
    );
  }

  Widget _eventNameEntryField() {
    return TextFormField(
      controller: _eventName,
      decoration: const InputDecoration(
          labelText: "Event name", hintText: 'Enter your event name'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter event name";
        }
        return null;
      },
    );
  }

  Widget _eventDescriptionEntryField() {
    return TextFormField(
      controller: _eventDescription,
      minLines: 2,
      maxLines: null,
      keyboardType: TextInputType.multiline,
      decoration: const InputDecoration(
          labelText: "Event description",
          hintText: 'Describe your event here',
          alignLabelWithHint: true),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please describe your event";
        }
        return null;
      },
    );
  }

  Widget _eventLocationEntryField() {
    return TextFormField(
      controller: _eventLocation,
      decoration: const InputDecoration(
          labelText: "Event location", hintText: 'Enter your event location'),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please enter event location";
        }
        return null;
      },
    );
  }

  Widget _eventDateEntryField() {
    return TextFormField(
      controller: _eventDate,
      decoration: const InputDecoration(
          labelText: "Event date", hintText: 'Choose your event date'),
      readOnly: true,
      onTap: () => _selectDate(context, DateTime.now()).then((date) => {
            if (date != null)
              _eventDate.text = '${date.year}-${date.month}-${date.day}'
          }),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please choose your event date";
        }
        return null;
      },
    );
  }

  Future<DateTime?> _selectDate(BuildContext context, DateTime initial) async {
    const daysInAYear = 365;
    const maxYearsTillEvent = 5;
    return await showDatePicker(
      context: context,
      initialDate: initial,
      firstDate: DateTime.now(),
      lastDate: DateTime.now().add(
        const Duration(days: daysInAYear * maxYearsTillEvent),
      ),
    );
  }

  Widget _getSectorEntryField() {
    return SingleChildScrollView(
      child: Row(
        children: [
          Flexible(
            flex: 2,
            child: _sectorNameEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _sectorRowsEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _sectorColumnsEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _sectorPriceEntryField(),
          ),
        ],
      ),
    );
  }

  Widget _getSectorButtons() {
    return Container(
      margin: const EdgeInsets.all(8.0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
          ElevatedButton(
            onPressed: sectors.isEmpty
                ? null
                : () => setState(
                      () {
                        sectors.removeLast();
                      },
                    ),
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.redAccent,
              fixedSize: const Size(90, 30),
            ),
            child: const Text('Remove'),
          ),
          ElevatedButton(
            onPressed: () => setState(
              () {
                try {
                  sectors.add(
                    Sector(
                      _sectorName.text,
                      double.parse(_sectorPrice.text),
                      int.parse(_sectorRows.text),
                      int.parse(_sectorColumns.text),
                    ),
                  );
                  _sectorName.text = '';
                  _sectorPrice.text = '';
                  _sectorRows.text = '';
                  _sectorColumns.text = '';
                } catch (e) {}
              },
            ),
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.blueAccent,
              fixedSize: const Size(90, 30),
            ),
            child: const Text('Add'),
          ),
        ],
      ),
    );
  }

  Widget _getSectorsList() {
    return SingleChildScrollView(
      child: Column(children: sectors.map((e) => _getSector(e)).toList()),
    );
  }

  Widget _getSector(Sector s) {
    return Text(
      s.toString(),
      style: const TextStyle(fontSize: 16),
    );
  }

  Widget _sectorNameEntryField() {
    return TextFormField(
      controller: _sectorName,
      decoration: const InputDecoration(labelText: "Sector name"),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "";
        }
        return null;
      },
    );
  }

  Widget _sectorRowsEntryField() {
    return TextFormField(
      controller: _sectorRows,
      decoration: const InputDecoration(labelText: "No of rows"),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "";
        }
        return null;
      },
    );
  }

  Widget _sectorColumnsEntryField() {
    return TextFormField(
      controller: _sectorColumns,
      decoration: const InputDecoration(labelText: "No of cols"),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "";
        }
        return null;
      },
    );
  }

  Widget _sectorPriceEntryField() {
    return TextFormField(
      controller: _sectorPrice,
      decoration: const InputDecoration(labelText: "Price \$"),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "";
        }
        return null;
      },
    );
  }

  Widget _submitButton() {
    return Container(
        margin: const EdgeInsets.only(top: 15.0),
        child: ElevatedButton(
          onPressed: () => print("Event created!"),
          child: const Text("Create"),
        ));
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
      drawer: const OrganizerNavigationDrawer(),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
          padding: const EdgeInsets.all(20),
          margin: const EdgeInsets.all(8),
          child: _getContent(),
        ),
      ),
    );
  }
}
