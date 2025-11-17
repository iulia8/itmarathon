import { Component, output } from '@angular/core';

import { CommonModalTemplate } from '../../../shared/components/modal/common-modal-template/common-modal-template';
import {
  ButtonText,
  ModalSubtitle,
  ModalTitle,
  PictureName,
  ButtonColor,
} from '../../../app.enum';

@Component({
  selector: 'app-participant-delete-modal',
  imports: [CommonModalTemplate],
  templateUrl: './participant-delete-modal.html',
  styleUrl: './participant-delete-modal.scss',
})
export class ParticipantDeleteModal {
  readonly closeModal = output<void>();
  readonly buttonAction = output<void>();

  public readonly pictureName = PictureName.StNick;
  public readonly title = ModalTitle.DeletingParticipant;
  public readonly buttonText = ButtonText.DeleteParticipant;
  public readonly subtitle = ModalSubtitle.DeleteInfo;

  public readonly buttonColor = ButtonColor.DeleteButton;

  public onCloseModal(): void {
    this.closeModal.emit();
  }

  public onActionButtonClick(): void {
    this.buttonAction.emit();
  }
}
