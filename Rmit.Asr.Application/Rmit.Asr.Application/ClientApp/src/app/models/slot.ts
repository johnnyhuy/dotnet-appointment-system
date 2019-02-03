import {Room} from "./room";
import {Student} from "./student";
import {Staff} from "./staff";

export interface Slot {
  room: Room,
  startTime: string,
  student: Student,
  staff: Staff
}
