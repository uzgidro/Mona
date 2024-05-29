part of 'hub_bloc.dart';

@immutable
sealed class HubState {}

final class HubInitial extends HubState {}

final class HubStarted extends HubState {}
