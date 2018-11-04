// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: packet.proto

#include "packet.pb.h"

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
class ConnectDefaultTypeInternal {
 public:
  ::google::protobuf::internal::ExplicitlyConstructed<Connect>
      _instance;
} _Connect_default_instance_;
class DisconnectDefaultTypeInternal {
 public:
  ::google::protobuf::internal::ExplicitlyConstructed<Disconnect>
      _instance;
} _Disconnect_default_instance_;
class LoginDefaultTypeInternal {
 public:
  ::google::protobuf::internal::ExplicitlyConstructed<Login>
      _instance;
} _Login_default_instance_;
}  // namespace packet
namespace protobuf_packet_2eproto {
static void InitDefaultsConnect() {
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  {
    void* ptr = &::packet::_Connect_default_instance_;
    new (ptr) ::packet::Connect();
    ::google::protobuf::internal::OnShutdownDestroyMessage(ptr);
  }
  ::packet::Connect::InitAsDefaultInstance();
}

::google::protobuf::internal::SCCInfo<0> scc_info_Connect =
    {{ATOMIC_VAR_INIT(::google::protobuf::internal::SCCInfoBase::kUninitialized), 0, InitDefaultsConnect}, {}};

static void InitDefaultsDisconnect() {
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  {
    void* ptr = &::packet::_Disconnect_default_instance_;
    new (ptr) ::packet::Disconnect();
    ::google::protobuf::internal::OnShutdownDestroyMessage(ptr);
  }
  ::packet::Disconnect::InitAsDefaultInstance();
}

::google::protobuf::internal::SCCInfo<0> scc_info_Disconnect =
    {{ATOMIC_VAR_INIT(::google::protobuf::internal::SCCInfoBase::kUninitialized), 0, InitDefaultsDisconnect}, {}};

static void InitDefaultsLogin() {
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  {
    void* ptr = &::packet::_Login_default_instance_;
    new (ptr) ::packet::Login();
    ::google::protobuf::internal::OnShutdownDestroyMessage(ptr);
  }
  ::packet::Login::InitAsDefaultInstance();
}

::google::protobuf::internal::SCCInfo<0> scc_info_Login =
    {{ATOMIC_VAR_INIT(::google::protobuf::internal::SCCInfoBase::kUninitialized), 0, InitDefaultsLogin}, {}};

void InitDefaults() {
  ::google::protobuf::internal::InitSCC(&scc_info_Connect.base);
  ::google::protobuf::internal::InitSCC(&scc_info_Disconnect.base);
  ::google::protobuf::internal::InitSCC(&scc_info_Login.base);
}

::google::protobuf::Metadata file_level_metadata[3];
const ::google::protobuf::EnumDescriptor* file_level_enum_descriptors[1];

const ::google::protobuf::uint32 TableStruct::offsets[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  ~0u,  // no _has_bits_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Connect, _internal_metadata_),
  ~0u,  // no _extensions_
  ~0u,  // no _oneof_case_
  ~0u,  // no _weak_field_map_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Connect, network_id_),
  ~0u,  // no _has_bits_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Disconnect, _internal_metadata_),
  ~0u,  // no _extensions_
  ~0u,  // no _oneof_case_
  ~0u,  // no _weak_field_map_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Disconnect, network_id_),
  ~0u,  // no _has_bits_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Login, _internal_metadata_),
  ~0u,  // no _extensions_
  ~0u,  // no _oneof_case_
  ~0u,  // no _weak_field_map_
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Login, network_id_),
  GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(::packet::Login, name_),
};
static const ::google::protobuf::internal::MigrationSchema schemas[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
  { 0, -1, sizeof(::packet::Connect)},
  { 6, -1, sizeof(::packet::Disconnect)},
  { 12, -1, sizeof(::packet::Login)},
};

static ::google::protobuf::Message const * const file_default_instances[] = {
  reinterpret_cast<const ::google::protobuf::Message*>(&::packet::_Connect_default_instance_),
  reinterpret_cast<const ::google::protobuf::Message*>(&::packet::_Disconnect_default_instance_),
  reinterpret_cast<const ::google::protobuf::Message*>(&::packet::_Login_default_instance_),
};

void protobuf_AssignDescriptors() {
  AddDescriptors();
  AssignDescriptors(
      "packet.proto", schemas, file_default_instances, TableStruct::offsets,
      file_level_metadata, file_level_enum_descriptors, NULL);
}

void protobuf_AssignDescriptorsOnce() {
  static ::google::protobuf::internal::once_flag once;
  ::google::protobuf::internal::call_once(once, protobuf_AssignDescriptors);
}

void protobuf_RegisterTypes(const ::std::string&) GOOGLE_PROTOBUF_ATTRIBUTE_COLD;
void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::internal::RegisterAllTypes(file_level_metadata, 3);
}

void AddDescriptorsImpl() {
  InitDefaults();
  static const char descriptor[] GOOGLE_PROTOBUF_ATTRIBUTE_SECTION_VARIABLE(protodesc_cold) = {
      "\n\014packet.proto\022\006packet\"\035\n\007Connect\022\022\n\nnet"
      "work_id\030\001 \001(\003\" \n\nDisconnect\022\022\n\nnetwork_i"
      "d\030\001 \001(\003\")\n\005Login\022\022\n\nnetwork_id\030\001 \001(\003\022\014\n\004"
      "name\030\002 \001(\t*8\n\004Type\022\010\n\004NONE\020\000\022\t\n\005LOGIN\020\001\022"
      "\013\n\007CONNECT\020\002\022\016\n\nDISCONNECT\020\003b\006proto3"
  };
  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
      descriptor, 196);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "packet.proto", &protobuf_RegisterTypes);
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
}  // namespace protobuf_packet_2eproto
namespace packet {
const ::google::protobuf::EnumDescriptor* Type_descriptor() {
  protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return protobuf_packet_2eproto::file_level_enum_descriptors[0];
}
bool Type_IsValid(int value) {
  switch (value) {
    case 0:
    case 1:
    case 2:
    case 3:
      return true;
    default:
      return false;
  }
}


// ===================================================================

void Connect::InitAsDefaultInstance() {
}
#if !defined(_MSC_VER) || _MSC_VER >= 1900
const int Connect::kNetworkIdFieldNumber;
#endif  // !defined(_MSC_VER) || _MSC_VER >= 1900

Connect::Connect()
  : ::google::protobuf::Message(), _internal_metadata_(NULL) {
  ::google::protobuf::internal::InitSCC(
      &protobuf_packet_2eproto::scc_info_Connect.base);
  SharedCtor();
  // @@protoc_insertion_point(constructor:packet.Connect)
}
Connect::Connect(const Connect& from)
  : ::google::protobuf::Message(),
      _internal_metadata_(NULL) {
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  network_id_ = from.network_id_;
  // @@protoc_insertion_point(copy_constructor:packet.Connect)
}

void Connect::SharedCtor() {
  network_id_ = GOOGLE_LONGLONG(0);
}

Connect::~Connect() {
  // @@protoc_insertion_point(destructor:packet.Connect)
  SharedDtor();
}

void Connect::SharedDtor() {
}

void Connect::SetCachedSize(int size) const {
  _cached_size_.Set(size);
}
const ::google::protobuf::Descriptor* Connect::descriptor() {
  ::protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages].descriptor;
}

const Connect& Connect::default_instance() {
  ::google::protobuf::internal::InitSCC(&protobuf_packet_2eproto::scc_info_Connect.base);
  return *internal_default_instance();
}


void Connect::Clear() {
// @@protoc_insertion_point(message_clear_start:packet.Connect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  // Prevent compiler warnings about cached_has_bits being unused
  (void) cached_has_bits;

  network_id_ = GOOGLE_LONGLONG(0);
  _internal_metadata_.Clear();
}

bool Connect::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!GOOGLE_PREDICT_TRUE(EXPRESSION)) goto failure
  ::google::protobuf::uint32 tag;
  // @@protoc_insertion_point(parse_start:packet.Connect)
  for (;;) {
    ::std::pair<::google::protobuf::uint32, bool> p = input->ReadTagWithCutoffNoLastTag(127u);
    tag = p.first;
    if (!p.second) goto handle_unusual;
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // int64 network_id = 1;
      case 1: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(8u /* 8 & 0xFF */)) {

          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int64, ::google::protobuf::internal::WireFormatLite::TYPE_INT64>(
                 input, &network_id_)));
        } else {
          goto handle_unusual;
        }
        break;
      }

      default: {
      handle_unusual:
        if (tag == 0) {
          goto success;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, _internal_metadata_.mutable_unknown_fields()));
        break;
      }
    }
  }
success:
  // @@protoc_insertion_point(parse_success:packet.Connect)
  return true;
failure:
  // @@protoc_insertion_point(parse_failure:packet.Connect)
  return false;
#undef DO_
}

void Connect::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // @@protoc_insertion_point(serialize_start:packet.Connect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteInt64(1, this->network_id(), output);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), output);
  }
  // @@protoc_insertion_point(serialize_end:packet.Connect)
}

::google::protobuf::uint8* Connect::InternalSerializeWithCachedSizesToArray(
    bool deterministic, ::google::protobuf::uint8* target) const {
  (void)deterministic; // Unused
  // @@protoc_insertion_point(serialize_to_array_start:packet.Connect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt64ToArray(1, this->network_id(), target);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), target);
  }
  // @@protoc_insertion_point(serialize_to_array_end:packet.Connect)
  return target;
}

size_t Connect::ByteSizeLong() const {
// @@protoc_insertion_point(message_byte_size_start:packet.Connect)
  size_t total_size = 0;

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()));
  }
  // int64 network_id = 1;
  if (this->network_id() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::Int64Size(
        this->network_id());
  }

  int cached_size = ::google::protobuf::internal::ToCachedSize(total_size);
  SetCachedSize(cached_size);
  return total_size;
}

void Connect::MergeFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_merge_from_start:packet.Connect)
  GOOGLE_DCHECK_NE(&from, this);
  const Connect* source =
      ::google::protobuf::internal::DynamicCastToGenerated<const Connect>(
          &from);
  if (source == NULL) {
  // @@protoc_insertion_point(generalized_merge_from_cast_fail:packet.Connect)
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
  // @@protoc_insertion_point(generalized_merge_from_cast_success:packet.Connect)
    MergeFrom(*source);
  }
}

void Connect::MergeFrom(const Connect& from) {
// @@protoc_insertion_point(class_specific_merge_from_start:packet.Connect)
  GOOGLE_DCHECK_NE(&from, this);
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  if (from.network_id() != 0) {
    set_network_id(from.network_id());
  }
}

void Connect::CopyFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_copy_from_start:packet.Connect)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void Connect::CopyFrom(const Connect& from) {
// @@protoc_insertion_point(class_specific_copy_from_start:packet.Connect)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool Connect::IsInitialized() const {
  return true;
}

void Connect::Swap(Connect* other) {
  if (other == this) return;
  InternalSwap(other);
}
void Connect::InternalSwap(Connect* other) {
  using std::swap;
  swap(network_id_, other->network_id_);
  _internal_metadata_.Swap(&other->_internal_metadata_);
}

::google::protobuf::Metadata Connect::GetMetadata() const {
  protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages];
}


// ===================================================================

void Disconnect::InitAsDefaultInstance() {
}
#if !defined(_MSC_VER) || _MSC_VER >= 1900
const int Disconnect::kNetworkIdFieldNumber;
#endif  // !defined(_MSC_VER) || _MSC_VER >= 1900

Disconnect::Disconnect()
  : ::google::protobuf::Message(), _internal_metadata_(NULL) {
  ::google::protobuf::internal::InitSCC(
      &protobuf_packet_2eproto::scc_info_Disconnect.base);
  SharedCtor();
  // @@protoc_insertion_point(constructor:packet.Disconnect)
}
Disconnect::Disconnect(const Disconnect& from)
  : ::google::protobuf::Message(),
      _internal_metadata_(NULL) {
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  network_id_ = from.network_id_;
  // @@protoc_insertion_point(copy_constructor:packet.Disconnect)
}

void Disconnect::SharedCtor() {
  network_id_ = GOOGLE_LONGLONG(0);
}

Disconnect::~Disconnect() {
  // @@protoc_insertion_point(destructor:packet.Disconnect)
  SharedDtor();
}

void Disconnect::SharedDtor() {
}

void Disconnect::SetCachedSize(int size) const {
  _cached_size_.Set(size);
}
const ::google::protobuf::Descriptor* Disconnect::descriptor() {
  ::protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages].descriptor;
}

const Disconnect& Disconnect::default_instance() {
  ::google::protobuf::internal::InitSCC(&protobuf_packet_2eproto::scc_info_Disconnect.base);
  return *internal_default_instance();
}


void Disconnect::Clear() {
// @@protoc_insertion_point(message_clear_start:packet.Disconnect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  // Prevent compiler warnings about cached_has_bits being unused
  (void) cached_has_bits;

  network_id_ = GOOGLE_LONGLONG(0);
  _internal_metadata_.Clear();
}

bool Disconnect::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!GOOGLE_PREDICT_TRUE(EXPRESSION)) goto failure
  ::google::protobuf::uint32 tag;
  // @@protoc_insertion_point(parse_start:packet.Disconnect)
  for (;;) {
    ::std::pair<::google::protobuf::uint32, bool> p = input->ReadTagWithCutoffNoLastTag(127u);
    tag = p.first;
    if (!p.second) goto handle_unusual;
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // int64 network_id = 1;
      case 1: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(8u /* 8 & 0xFF */)) {

          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int64, ::google::protobuf::internal::WireFormatLite::TYPE_INT64>(
                 input, &network_id_)));
        } else {
          goto handle_unusual;
        }
        break;
      }

      default: {
      handle_unusual:
        if (tag == 0) {
          goto success;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, _internal_metadata_.mutable_unknown_fields()));
        break;
      }
    }
  }
success:
  // @@protoc_insertion_point(parse_success:packet.Disconnect)
  return true;
failure:
  // @@protoc_insertion_point(parse_failure:packet.Disconnect)
  return false;
#undef DO_
}

void Disconnect::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // @@protoc_insertion_point(serialize_start:packet.Disconnect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteInt64(1, this->network_id(), output);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), output);
  }
  // @@protoc_insertion_point(serialize_end:packet.Disconnect)
}

::google::protobuf::uint8* Disconnect::InternalSerializeWithCachedSizesToArray(
    bool deterministic, ::google::protobuf::uint8* target) const {
  (void)deterministic; // Unused
  // @@protoc_insertion_point(serialize_to_array_start:packet.Disconnect)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt64ToArray(1, this->network_id(), target);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), target);
  }
  // @@protoc_insertion_point(serialize_to_array_end:packet.Disconnect)
  return target;
}

size_t Disconnect::ByteSizeLong() const {
// @@protoc_insertion_point(message_byte_size_start:packet.Disconnect)
  size_t total_size = 0;

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()));
  }
  // int64 network_id = 1;
  if (this->network_id() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::Int64Size(
        this->network_id());
  }

  int cached_size = ::google::protobuf::internal::ToCachedSize(total_size);
  SetCachedSize(cached_size);
  return total_size;
}

void Disconnect::MergeFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_merge_from_start:packet.Disconnect)
  GOOGLE_DCHECK_NE(&from, this);
  const Disconnect* source =
      ::google::protobuf::internal::DynamicCastToGenerated<const Disconnect>(
          &from);
  if (source == NULL) {
  // @@protoc_insertion_point(generalized_merge_from_cast_fail:packet.Disconnect)
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
  // @@protoc_insertion_point(generalized_merge_from_cast_success:packet.Disconnect)
    MergeFrom(*source);
  }
}

void Disconnect::MergeFrom(const Disconnect& from) {
// @@protoc_insertion_point(class_specific_merge_from_start:packet.Disconnect)
  GOOGLE_DCHECK_NE(&from, this);
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  if (from.network_id() != 0) {
    set_network_id(from.network_id());
  }
}

void Disconnect::CopyFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_copy_from_start:packet.Disconnect)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void Disconnect::CopyFrom(const Disconnect& from) {
// @@protoc_insertion_point(class_specific_copy_from_start:packet.Disconnect)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool Disconnect::IsInitialized() const {
  return true;
}

void Disconnect::Swap(Disconnect* other) {
  if (other == this) return;
  InternalSwap(other);
}
void Disconnect::InternalSwap(Disconnect* other) {
  using std::swap;
  swap(network_id_, other->network_id_);
  _internal_metadata_.Swap(&other->_internal_metadata_);
}

::google::protobuf::Metadata Disconnect::GetMetadata() const {
  protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages];
}


// ===================================================================

void Login::InitAsDefaultInstance() {
}
#if !defined(_MSC_VER) || _MSC_VER >= 1900
const int Login::kNetworkIdFieldNumber;
const int Login::kNameFieldNumber;
#endif  // !defined(_MSC_VER) || _MSC_VER >= 1900

Login::Login()
  : ::google::protobuf::Message(), _internal_metadata_(NULL) {
  ::google::protobuf::internal::InitSCC(
      &protobuf_packet_2eproto::scc_info_Login.base);
  SharedCtor();
  // @@protoc_insertion_point(constructor:packet.Login)
}
Login::Login(const Login& from)
  : ::google::protobuf::Message(),
      _internal_metadata_(NULL) {
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  name_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  if (from.name().size() > 0) {
    name_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.name_);
  }
  network_id_ = from.network_id_;
  // @@protoc_insertion_point(copy_constructor:packet.Login)
}

void Login::SharedCtor() {
  name_.UnsafeSetDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  network_id_ = GOOGLE_LONGLONG(0);
}

Login::~Login() {
  // @@protoc_insertion_point(destructor:packet.Login)
  SharedDtor();
}

void Login::SharedDtor() {
  name_.DestroyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
}

void Login::SetCachedSize(int size) const {
  _cached_size_.Set(size);
}
const ::google::protobuf::Descriptor* Login::descriptor() {
  ::protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages].descriptor;
}

const Login& Login::default_instance() {
  ::google::protobuf::internal::InitSCC(&protobuf_packet_2eproto::scc_info_Login.base);
  return *internal_default_instance();
}


void Login::Clear() {
// @@protoc_insertion_point(message_clear_start:packet.Login)
  ::google::protobuf::uint32 cached_has_bits = 0;
  // Prevent compiler warnings about cached_has_bits being unused
  (void) cached_has_bits;

  name_.ClearToEmptyNoArena(&::google::protobuf::internal::GetEmptyStringAlreadyInited());
  network_id_ = GOOGLE_LONGLONG(0);
  _internal_metadata_.Clear();
}

bool Login::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!GOOGLE_PREDICT_TRUE(EXPRESSION)) goto failure
  ::google::protobuf::uint32 tag;
  // @@protoc_insertion_point(parse_start:packet.Login)
  for (;;) {
    ::std::pair<::google::protobuf::uint32, bool> p = input->ReadTagWithCutoffNoLastTag(127u);
    tag = p.first;
    if (!p.second) goto handle_unusual;
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // int64 network_id = 1;
      case 1: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(8u /* 8 & 0xFF */)) {

          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::int64, ::google::protobuf::internal::WireFormatLite::TYPE_INT64>(
                 input, &network_id_)));
        } else {
          goto handle_unusual;
        }
        break;
      }

      // string name = 2;
      case 2: {
        if (static_cast< ::google::protobuf::uint8>(tag) ==
            static_cast< ::google::protobuf::uint8>(18u /* 18 & 0xFF */)) {
          DO_(::google::protobuf::internal::WireFormatLite::ReadString(
                input, this->mutable_name()));
          DO_(::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
            this->name().data(), static_cast<int>(this->name().length()),
            ::google::protobuf::internal::WireFormatLite::PARSE,
            "packet.Login.name"));
        } else {
          goto handle_unusual;
        }
        break;
      }

      default: {
      handle_unusual:
        if (tag == 0) {
          goto success;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, _internal_metadata_.mutable_unknown_fields()));
        break;
      }
    }
  }
success:
  // @@protoc_insertion_point(parse_success:packet.Login)
  return true;
failure:
  // @@protoc_insertion_point(parse_failure:packet.Login)
  return false;
#undef DO_
}

void Login::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // @@protoc_insertion_point(serialize_start:packet.Login)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    ::google::protobuf::internal::WireFormatLite::WriteInt64(1, this->network_id(), output);
  }

  // string name = 2;
  if (this->name().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->name().data(), static_cast<int>(this->name().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "packet.Login.name");
    ::google::protobuf::internal::WireFormatLite::WriteStringMaybeAliased(
      2, this->name(), output);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), output);
  }
  // @@protoc_insertion_point(serialize_end:packet.Login)
}

::google::protobuf::uint8* Login::InternalSerializeWithCachedSizesToArray(
    bool deterministic, ::google::protobuf::uint8* target) const {
  (void)deterministic; // Unused
  // @@protoc_insertion_point(serialize_to_array_start:packet.Login)
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    target = ::google::protobuf::internal::WireFormatLite::WriteInt64ToArray(1, this->network_id(), target);
  }

  // string name = 2;
  if (this->name().size() > 0) {
    ::google::protobuf::internal::WireFormatLite::VerifyUtf8String(
      this->name().data(), static_cast<int>(this->name().length()),
      ::google::protobuf::internal::WireFormatLite::SERIALIZE,
      "packet.Login.name");
    target =
      ::google::protobuf::internal::WireFormatLite::WriteStringToArray(
        2, this->name(), target);
  }

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()), target);
  }
  // @@protoc_insertion_point(serialize_to_array_end:packet.Login)
  return target;
}

size_t Login::ByteSizeLong() const {
// @@protoc_insertion_point(message_byte_size_start:packet.Login)
  size_t total_size = 0;

  if ((_internal_metadata_.have_unknown_fields() &&  ::google::protobuf::internal::GetProto3PreserveUnknownsDefault())) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        (::google::protobuf::internal::GetProto3PreserveUnknownsDefault()   ? _internal_metadata_.unknown_fields()   : _internal_metadata_.default_instance()));
  }
  // string name = 2;
  if (this->name().size() > 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::StringSize(
        this->name());
  }

  // int64 network_id = 1;
  if (this->network_id() != 0) {
    total_size += 1 +
      ::google::protobuf::internal::WireFormatLite::Int64Size(
        this->network_id());
  }

  int cached_size = ::google::protobuf::internal::ToCachedSize(total_size);
  SetCachedSize(cached_size);
  return total_size;
}

void Login::MergeFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_merge_from_start:packet.Login)
  GOOGLE_DCHECK_NE(&from, this);
  const Login* source =
      ::google::protobuf::internal::DynamicCastToGenerated<const Login>(
          &from);
  if (source == NULL) {
  // @@protoc_insertion_point(generalized_merge_from_cast_fail:packet.Login)
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
  // @@protoc_insertion_point(generalized_merge_from_cast_success:packet.Login)
    MergeFrom(*source);
  }
}

void Login::MergeFrom(const Login& from) {
// @@protoc_insertion_point(class_specific_merge_from_start:packet.Login)
  GOOGLE_DCHECK_NE(&from, this);
  _internal_metadata_.MergeFrom(from._internal_metadata_);
  ::google::protobuf::uint32 cached_has_bits = 0;
  (void) cached_has_bits;

  if (from.name().size() > 0) {

    name_.AssignWithDefault(&::google::protobuf::internal::GetEmptyStringAlreadyInited(), from.name_);
  }
  if (from.network_id() != 0) {
    set_network_id(from.network_id());
  }
}

void Login::CopyFrom(const ::google::protobuf::Message& from) {
// @@protoc_insertion_point(generalized_copy_from_start:packet.Login)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void Login::CopyFrom(const Login& from) {
// @@protoc_insertion_point(class_specific_copy_from_start:packet.Login)
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool Login::IsInitialized() const {
  return true;
}

void Login::Swap(Login* other) {
  if (other == this) return;
  InternalSwap(other);
}
void Login::InternalSwap(Login* other) {
  using std::swap;
  name_.Swap(&other->name_, &::google::protobuf::internal::GetEmptyStringAlreadyInited(),
    GetArenaNoVirtual());
  swap(network_id_, other->network_id_);
  _internal_metadata_.Swap(&other->_internal_metadata_);
}

::google::protobuf::Metadata Login::GetMetadata() const {
  protobuf_packet_2eproto::protobuf_AssignDescriptorsOnce();
  return ::protobuf_packet_2eproto::file_level_metadata[kIndexInFileMessages];
}


// @@protoc_insertion_point(namespace_scope)
}  // namespace packet
namespace google {
namespace protobuf {
template<> GOOGLE_PROTOBUF_ATTRIBUTE_NOINLINE ::packet::Connect* Arena::CreateMaybeMessage< ::packet::Connect >(Arena* arena) {
  return Arena::CreateInternal< ::packet::Connect >(arena);
}
template<> GOOGLE_PROTOBUF_ATTRIBUTE_NOINLINE ::packet::Disconnect* Arena::CreateMaybeMessage< ::packet::Disconnect >(Arena* arena) {
  return Arena::CreateInternal< ::packet::Disconnect >(arena);
}
template<> GOOGLE_PROTOBUF_ATTRIBUTE_NOINLINE ::packet::Login* Arena::CreateMaybeMessage< ::packet::Login >(Arena* arena) {
  return Arena::CreateInternal< ::packet::Login >(arena);
}
}  // namespace protobuf
}  // namespace google

// @@protoc_insertion_point(global_scope)