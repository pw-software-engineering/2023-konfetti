import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/sector.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/organizer/organizer_drawer.dart';

class EventEditPage extends StatefulWidget {
  const EventEditPage({Key? key, required this.event}) : super(key: key);
  final Event event;

  @override
  State<EventEditPage> createState() => _EventEditPageState();
}

class _EventEditPageState extends State<EventEditPage> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController _eventName = TextEditingController();
  final TextEditingController _eventDescription = TextEditingController();
  final TextEditingController _eventLocation = TextEditingController();
  final TextEditingController _eventDate = TextEditingController();
  final TextEditingController _eventTime = TextEditingController();
  final TextEditingController _sectorName = TextEditingController();
  final TextEditingController _sectorPrice = TextEditingController();
  final TextEditingController _sectorColumns = TextEditingController();
  final TextEditingController _sectorRows = TextEditingController();

  late Event _event;

  List<Sector> sectors = List.empty(growable: true);

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getGreeting(),
          Container(
              padding: const EdgeInsets.all(12.0),
              margin: const EdgeInsets.only(top: 20, bottom: 20),
              decoration: BoxDecoration(
                border: Border.all(
                  color: Theme.of(context).hintColor,
                ),
              ),
              child: Column(
                children: [
                  _getEventCreationForm(),
                ],
              )),
        ],
      ),
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
            _eventDateTimeEntryField(),
            _sectorEntryField(),
            _sectorButtons(),
            _sectorsList(),
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

  Widget _eventDateTimeEntryField() {
    return SingleChildScrollView(
      child: Row(
        children: [
          Flexible(
            flex: 1,
            child: _eventDateEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _eventTimeEntryField(),
          ),
        ],
      ),
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
              _eventDate.text =
                  '${date.year.toString()}-${date.month.toString().padLeft(2, '0')}-${date.day.toString().padLeft(2, '0')}'
          }),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return "Please choose your event date";
        }
        return null;
      },
    );
  }

  Widget _eventTimeEntryField() {
    return TextFormField(
      controller: _eventTime,
      decoration: const InputDecoration(
        labelText: "Event time",
        hintText: 'Choose your event time',
      ),
      readOnly: true,
      onTap: () => _selectTime(context).then((time) => {
            if (time != null)
              _eventTime.text =
                  '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}'
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

  Future<TimeOfDay?> _selectTime(BuildContext context) {
    return showTimePicker(
      initialTime: TimeOfDay.now(),
      context: context,
    );
  }

  Widget _sectorEntryField() {
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

  Widget _sectorButtons() {
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
                if (_sectorName.text.isEmpty) return;
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
              backgroundColor: Theme.of(context).primaryColorDark,
              fixedSize: const Size(90, 30),
            ),
            child: const Text('Add'),
          ),
        ],
      ),
    );
  }

  Widget _sectorsList() {
    return SingleChildScrollView(
      child: Column(children: sectors.map((e) => _sector(e)).toList()),
    );
  }

  Widget _sector(Sector s) {
    return Text(
      s.toString(),
      style: const TextStyle(fontSize: 16),
    );
  }

  Widget _sectorNameEntryField() {
    return TextFormField(
      controller: _sectorName,
      decoration: const InputDecoration(labelText: "Sector name"),
    );
  }

  Widget _sectorRowsEntryField() {
    return TextFormField(
      controller: _sectorRows,
      decoration: const InputDecoration(labelText: "No of rows"),
    );
  }

  Widget _sectorColumnsEntryField() {
    return TextFormField(
      controller: _sectorColumns,
      decoration: const InputDecoration(labelText: "No of cols"),
    );
  }

  Widget _sectorPriceEntryField() {
    return TextFormField(
      controller: _sectorPrice,
      decoration: const InputDecoration(labelText: "Price \$"),
    );
  }

  Widget _submitButton() {
    return Container(
      margin: const EdgeInsets.only(top: 15.0),
      child: ElevatedButton(
        onPressed: () => _submitEventEdit(),
        child: const Text("Update"),
      ),
    );
  }

  Future<void> _submitEventEdit() async {
    if (_formKey.currentState!.validate() && sectors.isNotEmpty) {
      try {
        _event.name = _eventName.text;
        _event.description = _eventDescription.text;
        _event.date = '${_eventDate.text}T${_eventTime.text}:00.000Z';
        _event.location = _eventLocation.text;
        _event.sectors = sectors;

        var response = await BackendCommunication().event.edit(_event);

        if (response.item2 != ResponseCode.allGood) {
          await _showDialogOnFailure(response.item2.name);
        } else {
          await _showDialogAfterEventCreation();
        }
      } catch (e) {
        await _showDialogOnFailure(e.toString());
      }

      if (!mounted) return;
      Navigator.of(context).popUntil((route) => route.isFirst);
    }
  }

  Future<void> _showDialogOnFailure(String errorMess) async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Something went wrong"),
          content: Text("Your request faced an error: $errorMess"),
          actions: [
            ElevatedButton(
              onPressed: () => {
                Navigator.pop(context),
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  Future<void> _showDialogAfterEventCreation() async {
    await showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text("Event has been updated"),
          content: const Text(
              "Your event has been updated! You can see it in 'My Events' tab."),
          actions: [
            ElevatedButton(
              onPressed: () => {
                Navigator.pop(context),
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  Text _getGreeting() {
    return Text(
      "Edit event",
      style: TextStyle(
        fontSize: 26,
        color: Theme.of(context).primaryColor,
        fontWeight: FontWeight.w600,
      ),
    );
  }

  @override
  void initState() {
    _event = widget.event;
    _eventName.text = _event.name;
    _eventDescription.text = _event.description;
    var dateIn = _event.date.split(" ")[0].split("/");
    String date = "${dateIn[2]}-${dateIn[0]}-${dateIn[1]}";
    var time = _event.date.split(" ")[1].split("T")[0].replaceAll(":00", "");
    _eventDate.text = date;
    _eventTime.text = time;
    _eventLocation.text = _event.location;
    sectors = _event.sectors;
    super.initState();
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
