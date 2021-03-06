// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: packet_type.proto

#include "packet_type.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/port.h>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/wire_format_lite_inl.h>
#include <google/protobuf/descriptor.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/reflection_ops.h>
#include <google/protobuf/wire_format.h>
// This is a temporary google only hack
#ifdef GOOGLE_PROTOBUF_ENFORCE_UNIQUENESS
#include "third_party/protobuf/version.h"
#endif
// @@protoc_insertion_point(includes)

namespace packet {
}  // namespace packet
namespace protobuf_packet_5ftype_2eproto {
void InitDefaults() {
}

const ::google::protobuf::EnumDescriptor* file_level_enum_descriptors[1];
const ::google::protobuf::uint32 TableStruct::offsets[1] = {};
static const ::google::protobuf::internal::MigrationSchema* schemas = NULL;
static const ::google::protobuf::Message* const* file_default_instances = NULL;

void protobuf_AssignDescriptors() {
  AddDescriptors();
  AssignDescriptors(
      "packet_type.proto", schemas, file_default_instances, TableStruct::offsets,
      NULL, file_level_enum_descriptors, NULL);
}

void protobuf_AssignDescriptorsOnce() {
  static ::google::protobuf::internal::once_flag once;
  ::google::protobuf::internal::call_once(once, protobuf_AssignDescriptors);
}

void protobuf_RegisterTypes(const ::std::string&) GOOGLE_PROTOBUF_ATTRIBUTE_COLD;
void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
}

void AddDescriptorsImpl() {
  InitDefaults();
  static const char descriptor[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
      "\n\021packet_type.proto\022\006packet*\264\004\n\004Type\022\010\n\004"
      "NONE\020\000\022\013\n\007CONNECT\020\002\022\016\n\nDISCONNECT\020\003\022\r\n\tE"
      "RROR_ACK\020\004\022\r\n\tLOGIN_REQ\020d\022\r\n\tLOGIN_ACK\020e"
      "\022\026\n\022ENTER_NEW_USER_REQ\020f\022\026\n\022ENTER_NEW_US"
      "ER_ACK\020g\022\014\n\010MOVE_REQ\020i\022\014\n\010MOVE_ACK\020j\022\022\n\016"
      "ENTER_ROOM_REQ\020k\022\022\n\016ENTER_ROOM_ACK\020l\022\025\n\021"
      "GET_ROOM_LIST_REQ\020m\022\025\n\021GET_ROOM_LIST_ACK"
      "\020n\022\021\n\rMAKE_ROOM_REQ\020o\022\021\n\rMAKE_ROOM_ACK\020p"
      "\022\026\n\022MOVE_ROOM_USER_REQ\020q\022\026\n\022MOVE_ROOM_US"
      "ER_ACK\020r\022\026\n\022EXIT_ROOM_USER_REQ\020s\022\026\n\022EXIT"
      "_ROOM_USER_ACK\020t\022\033\n\027ENTER_NEW_ROOM_USER_"
      "REQ\020u\022\033\n\027ENTER_NEW_ROOM_USER_ACK\020v\022\016\n\nLO"
      "GOUT_REQ\020w\022\016\n\nLOGOUT_ACK\020x\022\022\n\016CAST_SKILL"
      "_REQ\020y\022\022\n\016CAST_SKILL_ACK\020z\022\026\n\022CAST_SKILL"
      "_HIT_REQ\020{\022\026\n\022CAST_SKILL_HIT_ACK\020|b\006prot"
      "o3"
  };
  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
      descriptor, 602);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "packet_type.proto", &protobuf_RegisterTypes);
}

void AddDescriptors() {
  static ::google::protobuf::internal::once_flag once;
  ::google::protobuf::internal::call_once(once, AddDescriptorsImpl);
}
// Force AddDescriptors() to be called at dynamic initialization time.
struct StaticDescriptorInitializer {
  StaticDescriptorInitializer() {
    AddDescriptors();
  }
} static_descriptor_initializer;
}  // namespace protobuf_packet_5ftype_2eproto
namespace packet {
const ::google::protobuf::EnumDescriptor* Type_descriptor() {
  protobuf_packet_5ftype_2eproto::protobuf_AssignDescriptorsOnce();
  return protobuf_packet_5ftype_2eproto::file_level_enum_descriptors[0];
}
bool Type_IsValid(int value) {
  switch (value) {
    case 0:
    case 2:
    case 3:
    case 4:
    case 100:
    case 101:
    case 102:
    case 103:
    case 105:
    case 106:
    case 107:
    case 108:
    case 109:
    case 110:
    case 111:
    case 112:
    case 113:
    case 114:
    case 115:
    case 116:
    case 117:
    case 118:
    case 119:
    case 120:
    case 121:
    case 122:
    case 123:
    case 124:
      return true;
    default:
      return false;
  }
}


// @@protoc_insertion_point(namespace_scope)
}  // namespace packet
namespace google {
namespace protobuf {
}  // namespace protobuf
}  // namespace google

// @@protoc_insertion_point(global_scope)
