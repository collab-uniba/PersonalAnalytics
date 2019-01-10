//
//  EmotionPopUpWindowController.swift
//  PersonalAnalytics
//
//  Created by Luigi Quaranta on 03/01/2019.
//

import Cocoa
import Foundation
import CoreData

class EmotionPopUpWindowController: NSWindowController {

    // MARK: Properties

    // EmotionTracker instance
    var emotionTracker = EmotionTracker()

    // Buttons
    var valence: NSButton? = nil
    var arousal: NSButton? = nil
    @IBOutlet weak var activityPopupButton: NSPopUpButton!

    // Validation labels
    @IBOutlet weak var activityValidationLabel: NSTextField!
    @IBOutlet weak var valenceValidationLabel: NSTextField!
    @IBOutlet weak var arousalValidationLabel: NSTextField!

    // Other variables
    var firstTimeClick: Bool = true


    //------------------------------------------------------


    // MARK: Utility functions
    func resetForm() {
        // Reset buttons
        self.valence?.state = NSControl.StateValue.off
        self.valence = nil
        self.arousal?.state = NSControl.StateValue.off
        self.arousal = nil
        self.activityPopupButton.selectItem(at: -1)

        // Reset validation labels
        self.activityValidationLabel.isHidden = true
        self.valenceValidationLabel.isHidden = true
        self.arousalValidationLabel.isHidden = true

    }

    // MARK: Lifecycle events
    override func windowDidLoad() {
        super.windowDidLoad()
        resetForm()
    }

    //------------------------------------------------------

    // MARK: UI behaviour

    // Valence/Arousal radio buttons
    @IBAction func valenceRadioButtonsClicked(_ sender: NSButton) {
        self.valence = sender
    }
    @IBAction func arousalRadioButtonsClicked(_ sender: NSButton) {
        self.arousal = sender
    }

    // Repeat button
    @IBAction func repeatButtonClicked(_ sender: NSButton) {

        if let valenceValue = self.valence?.identifier?.rawValue,
            let arousalValue = self.arousal?.identifier?.rawValue,
            let activityValue = self.activityPopupButton.titleOfSelectedItem {

            if self.firstTimeClick == true {
                sender.title = NSLocalizedString("I have pushed the marker button!", tableName: "EmotionPopUp", comment: "Before continuing, recalls the user to push the marker button on the Empatica E4.")
                self.firstTimeClick = false
                return
            }

            let timestamp = NSDate()
            let activity = activityValue
            let valence = NSNumber(value: Int16(valenceValue)!)
            let arousal = NSNumber(value: Int16(arousalValue)!)

            let questionnaire = Questionnaire(timestamp: timestamp, activity: activity, valence: valence, arousal: arousal)

            emotionTracker.storeQuestionnaire(questionnaire: questionnaire)

            // Reset button state
            resetForm()

            // Request a new notification
            emotionTracker.scheduleNotification()

            // Close the window
            self.close()

            sender.title = NSLocalizedString("Oyu-tk-Vba.title", tableName: "EmotionPopUp", comment: "Original button title")
            self.firstTimeClick = true
        } else {

            // If the user didn't choose an option for the the activity...
            if self.activityPopupButton.titleOfSelectedItem == nil {
                activityValidationLabel.isHidden = false
            } else {
                activityValidationLabel.isHidden = true
            }

            // If the user didn't choose an option for the valence radios...
            if self.valence?.identifier?.rawValue == nil {
                valenceValidationLabel.isHidden = false
            } else {
                valenceValidationLabel.isHidden = true
            }

            // If the user didn't choose an option for the arousal radios...
            if self.arousal?.identifier?.rawValue == nil {
                arousalValidationLabel.isHidden = false
            } else {
                arousalValidationLabel.isHidden = true
            }

        }


    }


    @IBAction func exportToCsvClicked(_ sender: Any) {

        //Open SavePanel
        let savePanel = NSSavePanel()
        savePanel.title = "Export data to csv"
        savePanel.message = NSLocalizedString("Insert a name", tableName: "EmotionPopUp", comment:"How to call the output file when exporting to .csv")

        if (savePanel.runModal() == NSApplication.ModalResponse.OK) {
            let result = savePanel.url
            if (result != nil) {
                print("Saving to: ", result!.path)
                emotionTracker.exportToCsv(destinationPath: result!)
                print("Saved.")
            } else {
                return // User clicked cancel
            }

        }
    }
}
