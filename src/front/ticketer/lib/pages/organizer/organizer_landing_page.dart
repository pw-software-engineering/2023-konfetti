import 'package:flutter/material.dart';
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
    return await showDatePicker(
        context: context,
        initialDate: initial,
        firstDate: DateTime.now(),
        lastDate: DateTime(2101));
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
