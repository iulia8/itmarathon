import {
  Component,
  computed,
  HostBinding,
  inject,
  input,
  signal, OnInit,
} from '@angular/core';
import { User } from '../../../app.models';
import { ParticipantCard } from '../participant-card/participant-card';
import { toTimestamp } from '../../../utils/times';
import { UserService } from '../../../room/services/user';

@Component({
  selector: 'app-participant-list',
  imports: [ParticipantCard],
  templateUrl: './participant-list.html',
  styleUrl: './participant-list.scss',
})
export class ParticipantList implements OnInit {
  public readonly participants = input<User[]>([]);
  public readonly maxParticipants = input<number>(20);
  public readonly isAdmin = input<boolean>(false);
  public readonly userCode = input<string>('');
  public readonly allParticipants = signal<User[]>([]);

  readonly #userService = inject(UserService);

  @HostBinding('class.non-admin-list')
  get adminClass(): boolean {
    return !this.isAdmin();
  }

  currentCount = computed(() => this.participants().length);

  sortedParticipants = computed(() => {
    return [...this.participants()].sort(
      (firstParticipant, secondParticipant) => {
        if (firstParticipant.isAdmin !== secondParticipant.isAdmin)
          return firstParticipant.isAdmin ? -1 : 1;

        const firstJoinDate = toTimestamp(
          firstParticipant.createdOn ?? firstParticipant.modifiedOn
        );
        const secondJoinDate = toTimestamp(
          secondParticipant.createdOn ?? secondParticipant.modifiedOn
        );

        return firstJoinDate - secondJoinDate;
      }
    );
  });

  ngOnInit() {
    this.updateParticipantsList();
  }

  updateParticipantsList() {
    this.#userService.getUsers().subscribe((res) => {
      this.allParticipants.set(res.body ?? []);
    });
  }
}
