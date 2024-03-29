import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:ticketer/backend_communication/logic/communication.dart';
import 'package:ticketer/backend_communication/logic/user/communication_user.dart';
import 'package:ticketer/backend_communication/model/event.dart';
import 'package:ticketer/backend_communication/model/event_status.dart';
import 'package:ticketer/backend_communication/model/response_codes.dart';
import 'package:ticketer/backend_communication/model/user.dart';
import 'package:ticketer/pages/common/app_bar.dart';
import 'package:ticketer/pages/common/event_tile.dart';
import 'package:ticketer/pages/common/user_card.dart';
import 'package:ticketer/pages/user/user_drawer.dart';
import 'package:tuple/tuple.dart';

class UserLandingPage extends StatefulWidget {
  const UserLandingPage({Key? key}) : super(key: key);

  @override
  State<UserLandingPage> createState() => _UserLandingPageState();
}

class _UserLandingPageState extends State<UserLandingPage> {
  int _pageNo = 0;
  final int _pageSize = 3;
  bool _hasNextPage = true;
  final List<Event> _events = [];
  String filterLocation = "";
  String filterName = "";
  String filterEarlier = "";
  String filterLater = "";

  final TextEditingController _filterName = TextEditingController();
  final TextEditingController _filterLocation = TextEditingController();
  final TextEditingController _earlierThanEventDate = TextEditingController();
  final TextEditingController _earlierThanEventTime = TextEditingController();
  final TextEditingController _laterThanEventDate = TextEditingController();
  final TextEditingController _laterThanEventTime = TextEditingController();

  Future<void> _fetchMoreData() async {
    int page = _pageNo;
    final resOpened = await BackendCommunication().event.listFiltered(
        page,
        _pageSize,
        filterName,
        filterLocation,
        filterEarlier,
        filterLater,
        EventStatus.Opened);
    final resPublished = await BackendCommunication().event.listFiltered(
        page,
        _pageSize,
        filterName,
        filterLocation,
        filterEarlier,
        filterLater,
        EventStatus.Published);
    final resClosed = await BackendCommunication().event.listFiltered(
        page,
        _pageSize,
        filterName,
        filterLocation,
        filterEarlier,
        filterLater,
        EventStatus.Closed);

    setState(() {
      int before = _events.length;
      for (var ev in resPublished.item1.data["items"]) {
        _events.add(Event.fromJson(ev));
      }
      for (var ev in resOpened.item1.data["items"]) {
        _events.add(Event.fromJson(ev));
      }
      for (var ev in resClosed.item1.data["items"]) {
        _events.add(Event.fromJson(ev));
      }
      int after = _events.length;
      _hasNextPage = before != after;
      _pageNo++;
    });
  }

  Widget _getContent() {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          _getUserIcon(),
          _getGreeting(),
          _userCard(),
          _getEventFilter(),
          _getEventsList(),
        ],
      ),
    );
  }

  FutureBuilder _userCard() {
    return FutureBuilder<Tuple2<Response<dynamic>, ResponseCode>>(
      future: UserCommunication().view(),
      builder: (BuildContext context,
          AsyncSnapshot<Tuple2<Response<dynamic>, ResponseCode>> snapshot) {
        if (snapshot.hasData) {
          Tuple2<Response, ResponseCode>? data = snapshot.data;

          User user = User.fromJson(data?.item1.data);
          return UserCard(user: user);
        } else if (snapshot.hasError) {
          return Text("${snapshot.error}");
        }
        return const CircularProgressIndicator();
      },
    );
  }

  Text _getGreeting() {
    return Text(
      "Hello user",
      style: TextStyle(
        fontSize: 26,
        color: Theme.of(context).primaryColor,
      ),
    );
  }

  Container _getUserIcon() {
    return Container(
      height: 60,
      width: 60,
      decoration: BoxDecoration(
        shape: BoxShape.circle,
        color: Theme.of(context).primaryColor,
      ),
      alignment: Alignment.center,
      child: const Icon(
        Icons.person,
        color: Colors.white,
        size: 40,
      ),
    );
  }

  Widget _getEventsList() {
    return Column(
      children: [
        ListView.builder(
          shrinkWrap: true,
          itemBuilder: (_, index) {
            if (index < _events.length) {
              return EventTile(event: _events[index]);
            } else {
              try {
                _fetchMoreData();
                /*setState(() {
                  _pageNo++;
                });*/
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
          },
          itemCount: _hasNextPage ? _events.length + 1 : _events.length,
        ),
      ],
    );
  }

  Widget _getEventFilter() {
    return InkWell(
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
              Text(
                "Filters",
                style: TextStyle(
                    fontSize: 20, color: Theme.of(context).primaryColor),
              ),
              _eventNameEntryField(),
              _eventLocationEntryField(),
              _eventEarlierThanDateTimeEntryField(),
              _eventLaterThanDateTimeEntryField(),
              _submitButton(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _eventNameEntryField() {
    return TextFormField(
      controller: _filterName,
      decoration: const InputDecoration(
          labelText: "Event name", hintText: 'Enter event name'),
      validator: (value) {
        return null;
      },
    );
  }

  Widget _eventLocationEntryField() {
    return TextFormField(
      controller: _filterLocation,
      decoration: const InputDecoration(
          labelText: "Event location", hintText: 'Enter event location'),
      validator: (value) {
        return null;
      },
    );
  }

  Widget _eventEarlierThanDateTimeEntryField() {
    return SingleChildScrollView(
      child: Row(
        children: [
          Flexible(
            flex: 1,
            child: _eventEarlierThanDateEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _eventEarlierThanTimeEntryField(),
          ),
        ],
      ),
    );
  }

  Widget _eventEarlierThanDateEntryField() {
    return TextFormField(
      controller: _earlierThanEventDate,
      decoration: const InputDecoration(labelText: "Earlier than date"),
      readOnly: true,
      onTap: () => _selectDate(context, DateTime.now()).then((date) => {
            if (date != null)
              _earlierThanEventDate.text =
                  '${date.year.toString()}-${date.month.toString().padLeft(2, '0')}-${date.day.toString().padLeft(2, '0')}'
          }),
    );
  }

  Widget _eventEarlierThanTimeEntryField() {
    return TextFormField(
      controller: _earlierThanEventTime,
      decoration: const InputDecoration(labelText: "Earlier than time"),
      readOnly: true,
      onTap: () => _selectTime(context).then((time) => {
            if (time != null)
              _earlierThanEventTime.text =
                  '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}'
          }),
    );
  }

  Widget _eventLaterThanDateTimeEntryField() {
    return SingleChildScrollView(
      child: Row(
        children: [
          Flexible(
            flex: 1,
            child: _eventLaterThanDateEntryField(),
          ),
          Flexible(
            flex: 1,
            child: _eventLaterThanTimeEntryField(),
          ),
        ],
      ),
    );
  }

  Widget _eventLaterThanDateEntryField() {
    return TextFormField(
      controller: _laterThanEventDate,
      decoration: const InputDecoration(labelText: "Later than date"),
      readOnly: true,
      onTap: () => _selectDate(context, DateTime.now()).then((date) => {
            if (date != null)
              _laterThanEventDate.text =
                  '${date.year.toString()}-${date.month.toString().padLeft(2, '0')}-${date.day.toString().padLeft(2, '0')}'
          }),
    );
  }

  Widget _eventLaterThanTimeEntryField() {
    return TextFormField(
      controller: _laterThanEventTime,
      decoration: const InputDecoration(labelText: "Later than time"),
      readOnly: true,
      onTap: () => _selectTime(context).then((time) => {
            if (time != null)
              _laterThanEventTime.text =
                  '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}'
          }),
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

  Widget _submitButton() {
    return Container(
      margin: const EdgeInsets.only(top: 15.0),
      child: ElevatedButton(
        onPressed: () => _submitFilter(),
        child: const Text("Filter"),
      ),
    );
  }

  Future<void> _submitFilter() async {
    setState(() {
      filterLocation = _filterLocation.text;
      filterName = _filterName.text;
      if (_earlierThanEventDate.text.isNotEmpty) {
        if (_earlierThanEventTime.text.isNotEmpty) {
          filterEarlier =
              '${_earlierThanEventDate.text}T${_earlierThanEventTime.text}:00.000Z';
        } else {
          filterEarlier = '${_earlierThanEventDate.text}T00:00:00.000Z';
        }
      } else {
        filterEarlier = "";
      }
      if (_laterThanEventDate.text.isNotEmpty) {
        if (_laterThanEventTime.text.isNotEmpty) {
          filterLater =
              '${_laterThanEventDate.text}T${_laterThanEventTime.text}:00.000Z';
        } else {
          filterLater = '${_laterThanEventDate.text}T00:00:00.000Z';
        }
      } else {
        filterLater = "";
      }
      _events.clear();
      _pageNo = 0;
      _hasNextPage = true;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: ticketerAppBar("Welcome"),
      drawer: const UserNavigationDrawer(),
      body: Center(
        child: Container(
          constraints: const BoxConstraints(minWidth: 200, maxWidth: 500),
          padding: const EdgeInsets.all(20),
          child: _getContent(),
        ),
      ),
    );
  }
}
