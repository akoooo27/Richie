# Richie account settings: ship evaluation

Independent UI/UX evaluation of the **Richie Account Settings** canvas (Claude Design artifact `SQiXuTqyASonZHTVzVFW3y`, 51 boards, `account.css`, `canvas.json`) and its spec (`spec.md`, Revision 3) before handoff to engineering. Evaluated on 2026-11-05. I did not design or test this area, and I did not read `qa/`, `NOTES.md`, `CSS-REQUESTS.md` or `CANVAS-CHANGES.md`.

## Verdict

**Overall: 8.3 / 10, No-ship as a whole.** Six of the eight units can go to engineering now: one ships as drawn and five ship with fixes made during the build. Two units each have one Major issue that needs a design or product decision first. Both decisions are small and targeted, and neither needs the boards redrawn.

| # | Unit | Score | Verdict |
|---|---|---|---|
| 1 | Account shell | 8.5 | Ship with fixes |
| 2 | Profile | 8.2 | Ship with fixes |
| 3 | Sign-in and security | 8.3 | Ship with fixes |
| 4 | Confirm it’s you (step-up) | 8.3 | **No-ship** (1 Major: ST-1) |
| 5 | Preferences | 8.4 | Ship with fixes |
| 6 | Connected apps | 8.8 | Ship |
| 7 | Data and privacy | 8.1 | Ship with fixes |
| 8 | Account deletion lifecycle | 7.6 | **No-ship** (1 Major: DL-1) |

**Must fix before these two units ship:**

1. **ST-1 (step-up):** add a way forward when the code can’t be received. Today a person with no passkey who has lost access to the old mailbox reaches a dead end, and changing that email is exactly what they came to do.
2. **DL-1 (lifecycle):** define what the `/export` link token does after it is used or expires, and give people who lost the day-30 email a route back. As specified, "Request a new download" can land on the expired page.

---

## 1. Method

**Sources read in full:** `spec.md` (all sections), `how-we-design.md`, every board’s markup (`.design/canvas/project/*.dc.html`, with full-copy states diffed against their bases), the relevant parts of `account.css`, and the suiss guide and blocks catalog for each component that §8 names. I confirmed the on-disk project matches the published canvas: all 51 boards, `account.css` and `canvas.json` have the same byte sizes as the artifact’s published files.

**Rendering:** boards were built with `.design/qa-rig/build.mjs` into `.design/qa-out/eval/`. A second copy, `.design/qa-out/eval-dark/`, puts `.dark` on `<body>`, so every state, including those with no dark board, renders dark. This is needed because base boards draw light under `prefers-color-scheme: dark`, by design (spec §7.1).

**Automated pass** (`run.mjs`, port 9841): all 51 boards at 320×640, 375×812, 390×844, 768×1024, 1024×768, 1280×800, 1440×900, 1920×1080, 1280×540, and 640×400 (200 % zoom of 1280×800), in light and dark, with axe, overflow, clipping, target and keyboard passes. **Result: 0 blockers, 3 majors, 714 minors.** I verified every major by hand and discarded all three as rig artefacts (see "Discarded findings"). A forced-colours pass (port 9843) covered `Main`, `Security`, `Stepup-code-locked`, `Preferences`, `Connected-remove`, `Security-remove-passkey-mobile`, `Data-delete-confirm` and `Deletion-pending`.

**Manual pass** (my own Playwright scripts in `.design/qa-out/eval-results/scripts/`, port 9842):

- About 70 targeted screenshots: 1440, 1280, 768, 390 and 320; short heights 844×390 and 1280×540; forced dark; element crops at 2×.
- Tab-order traces on `Main`, `Preferences` (light and dark), `Security` (390), `Account-index` (390) and `Stepup-code-locked`.
- Layout-shift measurements: the y-position of the heading that follows each status slot, across base and state boards.
- A hit-area probe of row buttons with `elementFromPoint`.

Screenshots are in `.design/qa-out/eval-results/shots-eval/` (mine) and `.design/qa-out/eval-results/auto/shots/` (the rig). `.design/` is git-ignored, so the evidence is local.

All servers and browsers were shut down after the runs.

**Weights** (the same for every unit):

| Criterion | Weight |
|---|---|
| Visual design and hierarchy | 12 % |
| Consistency (within the area, with suiss) | 10 % |
| Usability and flow | 20 % |
| Content and copy | 10 % |
| Accessibility (WCAG 2.2 AA, drawn and specified) | 15 % |
| Responsive behaviour and theming | 10 % |
| State and edge-case coverage | 13 % |
| Handoff completeness | 10 % |

Unit score is the weighted mean, rounded to one decimal. The overall score is the unweighted mean of the eight units.

**Severity:**

- **Blocker:** prevents a core task, exposes data or security, fails WCAG 2.2 AA in a way that blocks people, or cannot be built without the designer.
- **Major:** a dead end, a missing recovery path, a misleading state, or a handoff contradiction likely to be built wrong. It needs a design or product decision before build.
- **Minor:** polish, copy, consistency or spec hygiene that a developer can fix during implementation without new design work.

**Ship bar:**

- **Ship:** no Blocker or Major, and any remaining Minor is optional.
- **Ship with fixes:** Minors only, and some must be made during the build.
- **No-ship:** any Blocker or Major outstanding.

**Discarded findings (verified false positives):**

- `focus/no-visible-focus-indicator` on `#stepup-code` (`Stepup-code-locked`): focus is drawn on the slot group by `.ra-otp.is-disabled:is(:focus-within)` (account.css:2648). It is visible in `shots-eval/Stepup-code-locked@1280x800@tab_1.png`.
- `clipped-text` on the Profile-extreme sidebar name: the spec’s intended 2-line clamp. Kept as Minor SH-2 for a separate glyph glitch.
- 641 `target-size/<44px`: at phone width, row `sm` buttons hit at about 46 px once scrolled into view (`elementFromPoint` 4 px outside the box still returns the button; the `::after` is `top/bottom: -6px`). The rig sampled points below the fold. Desktop 32 px row buttons meet AA 2.5.8 (24 px) by design.
- 63 axe `color-contrast` "needs review": these are text over alpha or overlay surfaces. The spec computes every pair (§7.1), and the renders show no low-contrast text.
- 10 `target-size/<24px` on the inline "Privacy Policy" link: exempt under 2.5.8 (inline).

---

## 2. Strengths

1. **A signature that belongs to this product.** The live "How customers see you" messenger explains why your name and photo matter. It is workspace-neutral, and the drawing is `aria-hidden` with a sr-only summary. It holds in dark, in forced colours and at 320 px. It is the only shadow and the only block of colour in the area, so it reads as intentional rather than decorative.
2. **State discipline that holds up when measured.** Reserved slots really do stop the page moving:
   - `#email-title` stays at y = 692 at 1440 across `Main`, `Profile-name-saved`, `Profile-save-failed` and `Profile-photo-removed`.
   - `#sessions-title` stays at y = 688 across `Security`, `-passkey-waiting` and `-passkey-failed`, and the same holds at 390 and 320.
   - Multi-step dialogs keep one height.
   - There is no horizontal overflow at any size from 320 px to 200 % zoom.
   - Axe found nothing critical or serious in either theme.
   - Forced colours keeps every shape, ring, switch and selected segment.
3. **Security-grade flows and copy, mapped to suiss.**
   - Step-up uses real numbers: "You have 3 tries left", and a countdown inside the commit button.
   - The locked input is `readonly` plus `aria-disabled`, so it is never dropped from focus.
   - Confirmations say what stops working.
   - Undo is inline rather than in a timed toast.
   - "Unavailable" never relies on opacity.
   - Contrast is computed for every pair.
   - §8 maps each recipe to a named suiss component, and the components it names exist in the guide.

---

## 3. Units

### Unit 1: Account shell (sidebar, mobile index, navigation and back)

Boards: `Account-sidebar`, `Account-index`, `Account-index-dark`, plus the shell inside every page board. Spec §3, §4.1.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.5 | 9.0 | 8.5 | 9.0 | 8.0 | 9.0 | 8.0 | 8.5 | **8.5** |

**What works**

- The suiss sidebar has a clear active state: fill plus weight plus icon colour, so it is not colour-only.
- The mobile index reads like iOS settings. Its neutral tiles and the long-email rule work at 320: "jam…@northwind.coffee" (`auto/shots/Account-index-dark/320x640-dark.png`).
- Back behaviour is specified: index links rather than `history.back()`, and Back closes dialogs first.
- At 768 the sidebar and a 440 px column work, with row actions wrapping as designed.
- One `nav` landmark wraps the sidebar, with no stray content outside it.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| SH-1 | Minor | **No skip link.** Eight sidebar stops (Back to Richie, 5 items, Help, Sign out) come before the page content on every desktop page. Landmarks technically satisfy 2.4.1, but sighted keyboard users tab through all 8 on every page. Add a "Skip to content" link, visible on focus, targeting the `h1`. | Rig keyboard order for `Main` at 1280×800 (stops 1–8 sidebar, 9 = Change photo), `eval-results/auto/report.md`; spec §3.2 and §4.1 are silent |
| SH-2 | Minor | **The sidebar truncation draws a half glyph.** The local-part ellipsis overlaps the last visible letter ("featherstonehau[g]…@"). The name clamps to 2 lines, which is acceptable because the full name is on Profile. Build the truncation with the `EmailText` component (`text-overflow`), not the canvas pseudo-element. | `shots-eval/Profile-extreme@2880x1800@zoom2@el_.ra-side-email.png`; account.css:1275; spec §4.0 long emails |

**Verdict: Ship with fixes** (add the skip link; build the email truncation without the glitch).

### Unit 2: Profile (photo, name, live preview, email change)

Boards: `Main`, `Profile-mobile`, `Profile-dark`, `Profile-name-saved`, `Profile-name-error`, `Profile-name-error-dark`, `Profile-save-failed`, `Profile-photo-crop`, `Profile-email-change`, `Profile-email-code`, `Profile-extreme`, `Profile-photo-removed`. Spec §4.2.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 7.5 | 8.5 | 7.5 | 9.0 | 8.5 | 7.5 | 9.0 | 8.5 | **8.2** |

**What works**

- The live preview is the area’s best idea.
- The fields are suiss filled inputs with a measured 3:1 edge.
- Remove photo gets an inline Undo with a focus rule.
- Save-failed keeps the typed "Jay" in the field and the preview, while the sidebar keeps the saved name, which is honest.
- The two-step change-email dialog keeps one height, and the code entry reuses identity’s.
- Extreme data wraps after the @ and never truncates the domain.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| PR-1 | Minor | **The reserved space shows as a hole.** In the clean state, the 36 px action row, the 40 px alert slot and the gaps leave about 150–170 px of blank space under the name hint at every width. At 1440 the hint ends at y ≈ 530 and the Email `h2` is at y = 705. At 390 the hint ends at ≈ 545 and the stage starts at ≈ 697. It reads as missing content and pushes the preview down on phones. Fix in build: collapse the alert slot until Save is pressed (an alert after an explicit action may move content), or keep Cancel and Save drawn as unavailable so the row carries content. | `shots-eval/Main@1440x900@dark@full.png`, `Main@390x844@full.png`, `Main@320x640@full.png`; spec §1.5 rule 8, §4.2 action row |
| PR-2 | Minor | **On phones the preview can’t be seen while typing.** At 390 the First name field is at y ≈ 360–404 and the preview’s name label at y ≈ 947. With an on-screen keyboard (about 300 px) the preview is off-screen. That contradicts spec §1.4: "On a phone, the preview is then directly below the field being typed in, above the keyboard". The signature feature is effectively desktop-only. Fixing PR-1 recovers part of this. Otherwise move the stage above the fields below 620 px, or add a compact name bubble. | `shots-eval/Main@390x844@full.png`; spec §1.4 (line 258) |
| PR-3 | Minor | **The crop dialog hides Zoom at short heights.** At 844×390 the crop area hits its 160 px floor and the Zoom slider scrolls out of view. Spec §4.2 and §7.2 row 26 size the crop "so the Zoom slider stays in view". The keyboard +/− still zooms, and the scroll cue is present. | `shots-eval/Profile-photo-crop@844x390.png`; spec line 673 |
| PR-4 | Minor | **Change-email step 1 looks unfinished.** The one-height rule leaves about 100 px of empty body between the field and the footer. Use it for something true, for example "We’ll also let your old address know." | `shots-eval/Profile-email-change@1280x800.png` |
| PR-5 | Minor | **Wrong popup semantics.** `aria-haspopup="dialog"` sits on Change photo and Upload photo, which open the OS file picker, not a dialog (the crop dialog only follows a pick). Drop it. | `Main.dc.html:44`, `Profile-photo-removed.dc.html:109` |
| PR-6 | Minor | **The first-name error moves the form.** The field error isn’t reserved, so it pushes everything below down 28 px (`#email-title` 692 → 720 at 1440, 1176 → 1204 at 390). That is normal, but the spec states "a state change never moves the content below it". Document field errors as the exception. | Measurement in method; spec §1.5 rule 8 |

Change email also depends on step-up; see **ST-1**. Profile ships with fixes on the assumption that ST-1 is resolved in the shared step-up.

**Verdict: Ship with fixes.**

### Unit 3: Sign-in and security (email codes, passkeys, devices, activity)

Boards: `Security`, `-mobile`, `-dark`, `-loading`, `-no-passkeys`, `-passkey-waiting`, `-passkey-failed`, `-remove-passkey`, `-remove-passkey-mobile`. Spec §4.3.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.0 | 9.0 | 8.5 | 8.0 | 8.5 | 8.0 | 8.5 | 8.0 | **8.3** |

**What works**

- The page structure is honest: email codes are "Always on", passkeys show provider and dates, and devices carry a "This device" badge.
- Neutral row triggers keep red for the two dedicated actions.
- Accessible names start with the visible label ("Remove the Pixel 8 passkey").
- Waiting and failed states hold the layout.
- The mobile action sheet works, including in forced colours.
- The empty passkeys state keeps one activity row, which is realistic.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| SE-1 | Minor | **Content jumps when the page loads.** At 320 the skeletons draw 1–2 lines where the loaded text wraps to 3–4. "Where you’re signed in" moves from y = 904 to 1032 and "Recent security activity" from 1424 to 1680 when data arrives. At 1440 the shift is 44 px, from the hidden Add a passkey button. That contradicts §4.3 ("so loaded rows don’t grow"). Size skeleton lines to the real line count at the current width, and reserve the foot button height. | `shots-eval/Security-loading@320x640@full.png` vs `Security@320x640@dark@full.png`; spec §4.3 lines 826–830 |
| SE-2 | Minor | **Uneven section rhythm.** Reserved, empty status slots add about 100 px (1440) to 130 px (320) between a section with a foot and the next heading, against about 55 px elsewhere. "Add a passkey" and "Sign out of other devices" float away from their groups. Consider a 1-line reserve at every width with the rare 2-line message allowed to grow, or a smaller gap after an empty foot. The same pattern applies to Preferences (PF-1). | `shots-eval/Security@1440x900@full.png`, `Security@320x640@dark@full.png` |
| SE-3 | Minor | **The confirm-to-step-up handoff is undefined.** Spec §4.3 Remove, "Then": step-up runs "inside the same dialog". But the confirm is `AlertDialog`/`AlertSheet` (340 px, `role=alertdialog`) and step-up is `Dialog`/`Drawer` (448 px); one component can’t be both. Where a failure alert sits inside an action sheet is also not drawn. Decide: close the confirm and open the step-up modal, with a stated focus return, or build Remove passkey on `Dialog` as the delete flow is. | Spec line 790, §8 rows 18–19; boards `Security-remove-passkey` and `Stepup` are drawn separately |
| SE-4 | Minor | **The add-passkey failure doesn’t say what to do.** "No passkey added. The prompt was canceled or timed out." gives no next step. That breaks the area’s own rule "Errors say what to do", and identity’s equivalent offers one. Add "Try again when you’re ready." | `Security-passkey-failed.dc.html:87`; spec §4.0 copy rules |
| SE-5 | Minor | **The unknown-device advice is incomplete.** "If you don’t recognize a device, sign it out and change your email." In a passwordless account, an unknown device means someone used your mailbox or a passkey. Add "check your passkeys", and note that changing email needs a code from that mailbox. | `Security.dc.html:92` |

**Verdict: Ship with fixes.**

### Unit 4: Confirm it’s you (step-up)

Boards: `Stepup`, `Stepup-mobile`, `Stepup-code`, `Stepup-code-error`, `Stepup-code-error-dark`, `Stepup-code-locked`. Spec §4.4.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.0 | 9.0 | 7.0 | 9.0 | 8.5 | 8.5 | 9.0 | 8.0 | **8.3** |

**What works**

- An in-place dialog keeps your place and names the action ("To remove the “Pixel 8” passkey…").
- Passkey comes first, with email as the alternative.
- The tries-left copy is real. The locked state moves the countdown into the commit button, which holds its width, so Cancel doesn’t jump.
- The input is `readonly` plus `aria-disabled` rather than `disabled`.
- The 8 slots fit at 320.
- The short-height scroll cue works at 844×390.
- Crockford normalisation and paste are specified.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| **ST-1** | **Major** | **Dead end when the code can’t be received.** Screen B offers only "Use a passkey instead" (when the account has one) and Cancel. There is no "Can’t get the code?", no help link and no explanation. The case that hits it is the change-email task itself: someone moving off a work address they’ve lost (they left the company), still signed in but past the 10-minute window, with no passkey. The code goes to the mailbox they can’t reach, and nothing tells them what to do. The brief requires recovery and no dead ends. **Fix:** add a plain "Can’t get the code?" under the code meta. It should explain the options: check spam, use a passkey if they have one, or contact Help under a stated recovery policy. Product must define that policy for a lost mailbox. Consider framing passkeys on Security as the way back in if you lose your email. | `Stepup-code.dc.html:42` (the only alternative); `shots-eval/Stepup-mobile@320x640.png`; spec §4.4 Screen B, §4.2 "Change email flow. Step-up first if needed" |
| ST-2 | Minor | **Screen A looks hollow.** It reserves screen B’s height, leaving about 80 px (1280×800) to 150 px (390) of empty space under the note. Acceptable for stability, but it looks unfinished. | `shots-eval/Stepup@1280x800.png`, `Stepup@390x844.png` |
| ST-3 | Minor | **A focus ring on static text.** The locked state draws a focus ring around a non-interactive error paragraph, which reads as a button. §7.4 says headings that take programmatic focus show no ring; treat the error message the same way. | `Stepup-code-locked.dc.html:38`; `shots-eval/Stepup-code-locked@1280x800@el_data-slot_dialog-content_.png`; spec §7.4 line 1495 |
| ST-4 | Minor | **Keyboard behaviour in drawers isn’t specified.** Nothing says how the step-up code, new-email, rename and typed-phrase drawers behave with the on-screen keyboard open. At 320×640 the delete drawer already fills the screen. §3.2 covers heights ≤540 only. Specify a `visualViewport`-driven max-height (or `interactive-widget=resizes-content`) and keep the commit button visible. | `shots-eval/Data-delete-confirm@320x640@dark.png`, `Stepup-mobile@320x640.png`; spec §3.2 |

**Verdict: No-ship** until ST-1 is resolved. Everything else here is among the strongest work in the area.

### Unit 5: Preferences

Boards: `Preferences`, `-mobile`, `-dark`, `-offline`. Spec §4.5.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 7.5 | 8.5 | 8.5 | 9.0 | 9.0 | 8.5 | 8.0 | 8.5 | **8.4** |

**What works**

- Segmented controls are real radio groups, a `fieldset` with a sr-only `legend`, and they show a visible selected edge.
- Language options carry `lang`.
- "Always on" is shown as text, not as a disabled switch.
- Auto-save gives per-section feedback, and the offline state reverts the change and explains it.
- Forced colours renders the switches and segments correctly.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| PF-1 | Minor | **Uneven gaps and a floating note.** Empty reserved status slots leave about 85 px (1440) to 125 px (320) between sections. The note "Every email from us also has an unsubscribe link." floats 50–90 px below its group and reads as orphaned. Same cause as SE-2. | `shots-eval/Preferences@1440x900@full.png`, `Preferences@320x640@full.png` |
| PF-2 | Minor | **The language reload isn’t fully specified.** Changing the language "reloads the interface" (§4.5), but nothing says where focus and scroll land afterwards, or how "Saved. Richie is now in…" survives the reload. Restore scroll and focus to the Language trigger, and render the status on load. | Spec §4.5 Language row |
| PF-3 | Minor | **Group semantics differ between sections.** Only "Emails from Richie" wraps its group in a `fieldset`, with the `h2` inside the `legend`; the other two sections don’t. Pick one pattern; `section` with `aria-labelledby` is enough. | `Preferences.dc.html:105` |

**Verdict: Ship with fixes.**

### Unit 6: Connected apps

Boards: `Connected`, `-mobile`, `-dark`, `-remove`, `-empty`. Spec §4.6.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.5 | 9.0 | 9.0 | 9.0 | 9.0 | 9.0 | 8.5 | 8.0 | **8.8** |

**What works**

- Scopes are in plain words ("Can see your name, email address and photo.").
- The confirm says what stops working and how to come back.
- The empty state explains how apps get here.
- After the last removal, focus goes to the empty-state title.
- The toast appears only because the row left the screen.
- Clean at every size and in both themes.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| CA-1 | Minor | **No rule for real app logos.** App identity is a letter tile only. Nothing says to use the client’s registered logo (Duende `LogoUri`) or to show its website or publisher, which is how people spot an app they don’t recognise. Optional. | `Connected.dc.html:33`; spec §4.6 |

**Verdict: Ship.**

### Unit 7: Data and privacy (download, delete)

Boards: `Data`, `-mobile`, `-dark`, `-ready`, `-delete-confirm`, `-delete-confirm-mobile`. Spec §4.7.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.0 | 7.5 | 8.5 | 8.0 | 8.5 | 8.0 | 8.0 | 7.5 | **8.1** |

**What works**

- The page has two clear sections.
- Deletion is never blocked.
- The delete dialog lists dated consequences ("Until December 5…", "Until January 4, 2027…").
- The typed-phrase field validates on press and is never dimmed.
- The drawer works on mobile.
- Ready-to-download is a real link with a size in its accessible name.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| DP-1 | Minor | **The board contradicts the spec on the delete footer.** The board renders a right-aligned form-dialog footer: Cancel is about 95 px wide and Delete account about 153 px. Spec §1.5 rule 5, §7.2 rows 18 and 31 and §8 row 31 call for `.is-even` equal halves from 768 px. `.is-even` appears in no board and in no CSS rule (grep: 0 hits). Pick one; the spec’s alert-dialog footer is the better choice for a confirmation. | `Data-delete-confirm.dc.html:37`; `shots-eval/Data-delete-confirm@1280x800.png`; spec lines 274, 1472, 1581 |
| DP-2 | Minor | **"sessions" breaks the one-name rule.** The download description uses "sessions", against the spec’s own rule (§4.0: a signed-in browser is a "device", not a "session"). Use "devices". | `Data.dc.html:34`, `Data-ready.dc.html:34` |
| DP-3 | Minor | **The biggest consequence is missing.** The deletion consequences never say you can’t use Richie with this account any more. A generic account-level line keeps to the product owner’s no-workspace rule. Flag to product, out of scope here: what happens to a workspace the person owns. | `Data-delete-confirm.dc.html` list; spec §4.7 |
| DP-4 | Minor | **Typed-phrase localisation isn’t specified.** Is the phrase translated (Language offers de, es, fr, pt-BR and ka), and does matching ignore case and extra spaces? | Spec §4.7 Field row |
| DP-5 | Minor | **The delete dialog is cramped at short heights.** At 200 % zoom (640×400) and at 844×390, the fixed header and footer leave about 130–150 px of scrolling body. The typed field, which has initial focus, starts out of view. Check that focus scrolls it into view in the build. | `auto/shots/Data-delete-confirm/zoom200-1280x800-light.png`, `shots-eval/Data-delete-confirm@844x390.png` |

**Verdict: Ship with fixes.**

### Unit 8: Account deletion lifecycle (pending sign-in, deleted screen, signed-out download)

Boards: `Data-deleted`, `Deletion-pending`, `Export-request`, `Export-request-mobile`, `Export-request-code`, `Export-requested`. Spec §4.8.

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Unit** |
|---|---|---|---|---|---|---|---|---|
| 8.0 | 8.5 | 6.5 | 8.0 | 8.5 | 8.5 | 7.0 | 6.5 | **7.6** |

**What works**

- The pages reuse identity’s solo frame, the shiba and the code entry, so a signed-out person recognises where they are.
- The timeline is legible: day 0, 30 and 60 are dated in every place.
- "Keep my account" is the obvious primary action, and it warns that apps must reconnect.
- The emails are written out.
- Renders are clean at 320–1440 and in forced dark.

**Issues**

| ID | Severity | Issue | Evidence |
|---|---|---|---|
| **DL-1** | **Major** | **The export link’s lifecycle is undefined and can dead-end.** §3.1 says "An expired or used token shows `/export/expired`". §4.8 says an expired download link before day 60 offers "**Request a new download** (to `/export`)", and the "ready" email promises "you can ask for a new one until January 4, 2027". If the day-30 token is single-use, as "used token" implies, that recovery route lands on the expired page. Nothing leads back to `/export` for someone who lost or never got the day-30 email. What identity sign-in shows for an address deleted in the last 30 days is not specified (§4.8 only says the email can create a new account). A developer has to guess token semantics and the sign-in branch. **Fix:** make the `/export` token reusable until day 60 (the email-code step already rate-limits it), or let the expired page send a fresh link to the address. Specify that sign-in or sign-up for a recently deleted address offers "Download your data from your deleted account". | Spec lines 374, 1069–1070, 1080, 1007; boards `Export-request`, `Export-requested` |
| DL-2 | Minor | **The pending page doesn’t say which account.** `Deletion-pending` has no account chip, unlike `Export-request`. Someone with a work and a personal account can’t tell which one is scheduled for deletion. Add the identity chip. | `Deletion-pending.dc.html:19`; `shots-eval/Deletion-pending@390x844@dark.png` |
| DL-3 | Minor | **"Keep my account" drops the return URL.** It always goes to the Richie app root (§4.8 Keep), discarding a deep link or an OAuth authorize request from another app. Continue to the original destination when there is one. | Spec §4.8 Deletion-pending, Keep row |
| DL-4 | Minor | **Copy-only states have inconsistent titles.** Several states are copy-only: the export code locked, expired and send-failed states, both `/export/expired` variants, the Keep failure and the sign-up error. The expired title differs between §3.1 ("This link has expired") and §4.8 (two variants). Reconcile the copy. | Spec §3.1 line 374, §4.8 lines 1065–1070 |
| DL-5 | Minor | **Tone.** The cheerful, tongue-out shiba heads "Your account will be deleted on December 5". Identity marks failure states with a badge; use a neutral pose or a badge here. | `shots-eval/Data-deleted@1440x900.png` |

**Verdict: No-ship** until DL-1 is decided.

---

## 4. Cross-cutting handoff notes (Minor)

- **X-1:** The spec admits two sources of truth:
  - §6.2 says its size and position tables are stale ("Pending sync R-08, R-10… `canvas.json` wins").
  - §7.2 says "where the built `account.css` and this table differ, this table wins", and DP-1 shows real drift.
  - 81 lines carry internal QA IDs (A-xx, B-xx, C-xx, R-xx) that point to documents engineering won’t read.

  Before handoff, sync the tables to `canvas.json`, reconcile `.is-even`, and move the IDs to a changelog.
- **X-2:** §8 row 18 says "Set `dismissible` so an outside click doesn’t close it". That reads backwards: in suiss, `dismissible` enables backdrop dismissal, and the confirm pattern defaults it to `false` (catalog §2.8). Write `dismissible={false}`.
- **Theme:** base boards draw light regardless of `prefers-color-scheme` (spec §7.1, documented), with `.dark` set by the app’s theme script. The forced-dark renders of every state look correct, so this is not an issue. Note that only Profile, Profile-name-error, Security, Step-up, Preferences, Connected, Data and the index have drawn dark boards.

---

## 5. Must-fix (blocks shipping)

1. **ST-1 (Major):** add a "Can’t get the code?" path to step-up screen B and define the lost-mailbox recovery policy, so changing an email you can no longer read is not a dead end.
2. **DL-1 (Major):** define `/export` token reuse and expiry so "Request a new download" can’t land on the expired page. Add a route back to the export for people who lost the day-30 email, from identity sign-in or sign-up for a recently deleted address.

## 6. Fix in implementation (units shipping with fixes)

These need no new design work:

- **Shell:** SH-1 (skip link), SH-2 (email truncation).
- **Profile:** PR-1 (the clean-state void), PR-2 (phone preview placement), PR-3, PR-5.
- **Security:** SE-1 (skeleton heights), SE-3 (decide the confirm-to-step-up transition), SE-4 (failure copy).
- **Preferences:** PF-2 (language reload focus and status).
- **Data and privacy:** DP-1 (one footer rule), DP-2 ("devices").

The remaining Minors are recommended polish.

---

# Round 2 (2026-11-05)

Re-evaluation of the team’s fix pass: canvas version `1793860937-9254` (56 boards) and spec Revision 4. Same independence, method, rubric, weights and ship bar as round 1. As before, I did not read `qa/`, `NOTES.md`, `CSS-REQUESTS.md` or `CANVAS-CHANGES.md`. Spec §6.2 is being regenerated, so it was excluded, as asked.

## R2.1 Verdict

**Overall: 8.6 / 10, Ship with fixes.** Both round 1 Majors are resolved, and no new Blocker or Major was found. Every unit can go to engineering. The remaining items are Minors to fix during the build, most of them spec-versus-board mismatches introduced by the fix pass.

| # | Unit | Round 1 | Round 2 | Verdict |
|---|---|---|---|---|
| 1 | Account shell | 8.5 · Ship with fixes | **8.7** | Ship |
| 2 | Profile | 8.2 · Ship with fixes | **8.4** | Ship with fixes |
| 3 | Sign-in and security | 8.3 · Ship with fixes | **8.6** | Ship with fixes |
| 4 | Confirm it’s you | 8.3 · **No-ship** | **8.7** | Ship with fixes |
| 5 | Preferences | 8.4 · Ship with fixes | **8.6** | Ship |
| 6 | Connected apps | 8.8 · Ship | **8.9** | Ship |
| 7 | Data and privacy | 8.1 · Ship with fixes | **8.2** | Ship with fixes |
| 8 | Deletion lifecycle | 7.6 · **No-ship** | **8.5** | Ship with fixes |

**Nothing blocks shipping.** Fix these in the build:

- **N2-01:** follow the spec, not the board: in the delete dialog the consequences list comes before the typed field.
- **N2-06:** `Signin-deleted` "Download your data" goes to `Export-start` with the address prefilled, not to `Export-request`.
- **N2-02:** update the spec to describe the mobile Profile order the CSS now ships.
- **N2-03:** on screen C, hide the passkey bullet when the account has no passkey.
- **N2-05:** settle the initial focus for step-up opened from a confirm.

## R2.2 Method

**Sync check.** All 56 boards, `account.css` (90,581 bytes) and `canvas.json` (14,533 bytes) on disk match the published artifact’s file list byte for byte.

**Builds:** `.design/qa-out/eval-2/`, plus a forced-dark copy at `.design/qa-out/eval-2-dark/`.

**Change map.** I diffed each built board against round 1’s build (`.design/qa-out/eval/`), with SVGs stripped:

- 51 boards changed. Most changes are the skip link, `id="page-title"` and copy.
- The step-up and Preferences boards were restructured.
- 5 boards are new.

**Automated pass** (port 9851; same 10 viewports × 2 themes): **0 blockers, 5 majors, 777 minors.** I discarded all 5 majors after checking each by hand:

| Major (rig) | Why it was discarded |
|---|---|
| `#stepup-code` no visible focus (locked state) | The group ring is visible on focus: `shots/Stepup-code-locked@1280x800@tab_1@el_data-slot_dialog-content_.png` |
| 2 × Profile-extreme sidebar name clipped | The intended 2-line clamp, as in round 1 |
| 2 × text overlap in `Data-delete-confirm` at 320 | Body text scrolled under the sticky footer, which the rig README lists as known noise. The scroll hairline shows in `shots/Data-delete-confirm@320x640.png` |

The minors are the same categories as round 1 (target-size probes, axe needs-review, the exempt inline link).

**Forced colours** (port 9853): `Stepup-cant-get-code` (and mobile), `Export-start`, `Signin-deleted`, `Deletion-pending`, `Main`, `Data-delete-confirm` and `Stepup-code-locked`. No real findings.

**Manual pass** (port 9852):

- About 40 screenshots of new and changed boards: 1440, 1280, 390, 320, 844×390, forced dark.
- Tab traces: the skip link (1280 and 390), screen C, step-up at 390, and `Export-start`.
- Layout-shift measurements: loading versus loaded, and the Profile states.

Evidence is under `.design/qa-out/eval-2-results/` (`shots/`, `auto/`, `forced/`, `scripts/`). Every server and browser was shut down after the runs.

## R2.3 Round 1 issues: status

| ID | Sev (R1) | Status | Evidence |
|---|---|---|---|
| SH-1 | Minor | **Resolved** | "Skip to content" is the first Tab stop on every shell page, a 44 px outline pill with the ring, targeting `#page-title` (`shots/Main@1280x800@tab_1.png`, and the same at 390 and in dark); spec §3.2 line 433 |
| SH-2 | Minor | **Resolved** | `text-overflow` ellipsis with no half glyph (`shots/Profile-extreme@2880x1800@zoom2@el_.ra-side-email.png`). Nit: about a 6 px gap remains between "…" and "@" |
| PR-1 | Minor | **Partly** | From a 420 px column the slots reserve 1 line (account.css: `@container (min-width: 420px) and (max-width: 619px)`). `#email-title` moved 692 → 676 at 1440 and 1176 → 1148 at 390. A ~150 px blank block under the name hint remains at desktop (`shots/Main@1280x800@tab_1.png`) and below Last name on phones |
| PR-2 | Minor | **Resolved, with new N2-02** | Below a 620 px column the stage now sits directly under First name (CSS `order`). At 390 the preview starts about 20 px under the field (`shots/Main@390x844@full.png`) |
| PR-3 | Minor | **Resolved** (documented exception) | Spec §1.5 rule 8, line 318; §4.2 line 729 |
| PR-4 | Minor | **Partly** | New description: "…Your current address keeps working until then." The ~100 px empty body in step 1 remains (`shots/Profile-email-change@1280x800.png`) |
| PR-5 | Minor | **Resolved** | `aria-haspopup="dialog"` is removed from Change photo and Upload photo (0 hits) |
| PR-6 | Minor | **Resolved** (documented exception) | Spec line 317 |
| SE-1 | Minor | **Resolved** | Loading and loaded positions are identical. `#sessions-title`: 688/688 at 1440, 1032/1032 at 320. `#activity-title`: 1116/1116, 1700/1700. The skeletons add width-dependent lines and a foot spacer |
| SE-2 | Minor | **Not resolved** | Section gaps are unchanged (`#sessions-title` still at 688 at 1440); `shots/Security@1440x900@full.png` |
| SE-3 | Minor | **Resolved, with new N2-05** | Spec §4.0 hand-off (lines 552–558), §7.4 line 1630, §8 "Confirm → step-up" |
| SE-4 | Minor | **Resolved** | "The passkey prompt closed. Try again." (`Security-passkey-failed`) |
| SE-5 | Minor | **Resolved** | "Don’t recognize a device? Sign it out, then check your passkeys and your email account’s security." |
| **ST-1** | **Major** | **Resolved** | Screen B now offers "Can’t get the code?", which opens screen C in the same dialog at the same height. Screen C gives spam, wait and resend, and passkey options, then a support route for a lost mailbox ("Contact support", a new tab, topic and signed-in account attached). Back returns to screen B with the code, tries and timer unchanged. Boards `Stepup-cant-get-code` and `-mobile`; spec §4.4 lines 943–955, §9 Q7 |
| ST-2 | Minor | **Not resolved** | Screen A still has about 90 px (1280) to 170 px (390) of empty space under the note (`shots/Stepup@1280x800.png`, `shots/Stepup@390x844.png`) |
| ST-3 | Minor | **Resolved** | No ring on the locked error (`shots/Stepup-code-locked@1280x800@el_data-slot_dialog-content_.png`); spec §7.4 line 1620 |
| ST-4 | Minor | **Resolved** | Spec §4.0 lines 559–563 (`visualViewport`, footer above the keyboard, field scrolls into view) |
| PF-1 | Minor | **Not resolved** | The gaps and the floating unsubscribe note are unchanged (`shots/Preferences@320x640@full.png`) |
| PF-2 | Minor | **Resolved** | Spec §4.5 line 976: reload in place, the section scrolled into view, focus on the Language trigger |
| PF-3 | Minor | **Resolved** | The `fieldset`/`legend` is gone; all three sections use the same `section` + `h2` pattern |
| CA-1 | Minor | **Resolved** | Spec §4.6 line 1016, §8 (`LogoUri`, with the letter tile as fallback) |
| DP-1 | Minor | **Resolved** | The spec now classes the delete dialog as a form dialog with a right-aligned footer; `.is-even` is withdrawn (§1.5 rule 5, line 305). Board and spec agree |
| DP-2 | Minor | **Resolved** | "devices" in `Data` and `Data-ready` |
| DP-3 | Minor | **Resolved** | "After that, you can’t sign in, use Richie with this account or get it back." |
| DP-4 | Minor | **Resolved** | Spec line 1064: trim, collapse spaces, ignore case; the phrase is localised |
| DP-5 | Minor | **Partly, with new N2-01** | The spec scrolls the field into view (line 1064), but the board moved the field above the consequences list instead |
| **DL-1** | **Major** | **Resolved** | The day-30 link is reusable until day 60, and each use needs a fresh code (spec lines 1082–1090). "Request a new download" always reaches `/export`, never an expired page. New `Export-start` covers people with no link, and `Signin-deleted` catches them at identity sign-in. Boards `Export-start`, `-mobile` and `Signin-deleted` |
| DL-2 | Minor | **Resolved** | The account chip is on `Deletion-pending` (`shots/Deletion-pending@390x844.png`) |
| DL-3 | Minor | **Resolved** | Keep continues to the sign-in’s return URL (spec line 1114) |
| DL-4 | Minor | **Resolved** | The `h1` and the document title now match for both expired cases (spec lines 1157–1162) |
| DL-5 | Minor | **Resolved** | The shiba is a 1× (31 × 39) brand mark on every solo page (`shots/Data-deleted@1440x900.png`) |
| X-1 | Minor | **Not resolved** | §7.2 still says "where the built `account.css` and this table differ, this table wins" (line 1527). Lines carrying internal QA IDs rose from 81 to 142. §6.2 was excluded this round |
| X-2 | Minor | **Resolved** | §8 row 18: `dismissible={false}` |

**Totals** (33 round 1 issues): 26 resolved, 3 partly, 4 not resolved. Both Majors are resolved.

## R2.4 New issues

| ID | Unit | Severity | Issue | Evidence |
|---|---|---|---|---|
| N2-01 | Data and privacy | Minor | **The board asks for confirmation before showing what happens.** The delete dialog board moved the typed field *above* the consequences list. The spec keeps the list first and scrolls the field into view (§4.7 lines 1060–1064). At 320 the last consequence is below the fold, and with a keyboard open none would show. Build it as specified, and fix the board so no one copies it. | `Data-delete-confirm.dc.html` (field before `ul#delete-dlg-list`); `shots/Data-delete-confirm@1280x800.png`, `@320x640.png` |
| N2-02 | Profile | Minor | **The new mobile Profile order isn’t in the spec.** It fixes PR-2, but the spec still says the stage stacks *under* the name fields (§1.4 line 288, §4.2 line 687). The new order also separates First name and Last name by about 470 px of preview at 390, which leaves the shared hint far from First name. Update the spec, and consider a compact preview below 620 px. | `shots/Main@390x844@full.png`; account.css, the `@container (max-width: 619px)` block (comment cites PR-2) |
| N2-03 | Confirm it’s you | Minor | **Screen C has a dead bullet for its main audience.** It always lists "Use a passkey instead, if you’ve set one up.", but the button shows only when a passkey exists. For the person screen C is for (no passkey) the bullet does nothing; hide it. Also, §4.4 line 902 still says "The title is the same in every state", but screen C is titled "Can’t get the code?". | Spec lines 950–951, 902; `Stepup-cant-get-code.dc.html` |
| N2-04 | Sign-in and security | Minor | **The passkey recovery nudge from Q7 is missing.** Q7’s "Security should also suggest adding a passkey as the way back in if email is lost" isn’t in any copy; the empty passkeys row still reads "…instead of waiting for a code." | Spec line 1729 vs line 815; `Security-no-passkeys` |
| N2-05 | Confirm it’s you | Minor | **Initial focus differs by entry path, and the boards don’t show the rule.** Remove-passkey step-up is always opened from a confirm, so by SE-3 it focuses its title (§4.0 line 555, §7.4 line 1630). The boards draw arrival focus on "Use a passkey" (`Stepup`) and on the code input (`Stepup-code`), and §4.4 line 931 still says the code input "autofocuses". State the rule per entry path, and annotate or redraw. | `Stepup.dc.html:27`, `Stepup-code.dc.html:29` |
| N2-06 | Deletion lifecycle | Minor | **The `Signin-deleted` board links to the wrong page.** "Download your data" points to `Export-request.dc.html`, the token page with the "address comes from the email link" chip. The spec sends it to `Export-start` with the address prefilled. | `Signin-deleted.dc.html:23`; spec line 1170 |
| N2-07 | Deletion lifecycle | Minor | **Precedence over a new account is unstated.** If the person has already used "Create a new account" with the same address during days 30–60, `Signin-deleted` is defined only "for an address whose account was deleted in the last 30 days", so read literally it would intercept sign-in to the new account. State that a live account wins and that old data stays reachable through `Export-start`. | Spec lines 1164, 1170 |
| N2-08 | Deletion lifecycle | Minor | **`Export-start` reveals deletions, with no rate limit stated.** Its error "There’s no deleted Richie account with data to download for this address…" reveals whether an address had an account deleted in the last 30 days. The trade-off is accepted for `Signin-deleted` (line 1171), but neither the acceptance nor a rate limit is stated for `Export-start`. | Spec line 1127 |

## R2.5 Product working assumptions

**Support-assisted recovery for a lost mailbox (§9 Q7).** The design handles it well:

- Screen C is calm and specific.
- It keeps the dialog’s height and the code state.
- It names the address.
- Contact support carries the topic and the signed-in account.
- The policy notifies both addresses and adds 7 days of mandatory step-up.

**Product must confirm:**

1. **The written verification procedure** and who may change an email. Support-led recovery is the main account-takeover route for passwordless accounts.
2. **How support replies.** The form must collect a reachable contact, not the dead mailbox.
3. **An expected response time,** shown on screen C (for example "We usually reply within 1 business day").
4. **The same route at identity sign-in.** A signed-out person with no passkey and no mailbox can’t reach step-up at all. This is a handoff item for the identity canvas.

**Reusable day-30 link until day 60.** The design handles this well too:

- Each use requires a fresh email code, so the link alone downloads nothing.
- "Request a new download" can never dead-end.
- `Export-start` and `Signin-deleted` cover people who lost the link.

**Product and security must confirm:**

1. The token is bound to the address and revoked at day 60.
2. Rate limits on `Export-start` and on code sends.
3. The enumeration trade-off on `Export-start` and `Signin-deleted` (N2-08).
4. The still-open legal check on keeping data for 60 days (§9 Q3).

## R2.6 Scorecards (round 2)

The weights are unchanged (visual 12, consistency 10, usability 20, content 10, accessibility 15, responsive 10, states 13, handoff 10). Scores are rounded half up; the lifecycle computes to 8.45.

| Unit | Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Score** | Verdict |
|---|---|---|---|---|---|---|---|---|---|---|
| Account shell | 8.5 | 9.0 | 8.5 | 9.0 | 9.0 | 9.0 | 8.0 | 9.0 | **8.7** | Ship |
| Profile | 7.5 | 8.5 | 8.5 | 9.0 | 8.5 | 8.0 | 9.0 | 8.0 | **8.4** | Ship with fixes |
| Sign-in and security | 8.0 | 9.0 | 8.5 | 9.0 | 8.5 | 8.5 | 9.0 | 8.5 | **8.6** | Ship with fixes |
| Confirm it’s you | 8.0 | 8.5 | 9.0 | 9.0 | 9.0 | 8.5 | 9.5 | 8.0 | **8.7** | Ship with fixes |
| Preferences | 7.5 | 9.0 | 8.5 | 9.0 | 9.0 | 8.5 | 8.5 | 9.0 | **8.6** | Ship |
| Connected apps | 8.5 | 9.0 | 9.0 | 9.0 | 9.0 | 9.0 | 8.5 | 9.0 | **8.9** | Ship |
| Data and privacy | 8.0 | 8.5 | 8.0 | 9.0 | 8.5 | 8.5 | 8.0 | 7.5 | **8.2** | Ship with fixes |
| Deletion lifecycle | 8.5 | 8.5 | 8.5 | 8.5 | 8.5 | 8.5 | 8.5 | 8.0 | **8.5** | Ship with fixes |

**Why each verdict:**

- **Shell: Ship.** No open issues.
- **Preferences: Ship.** PF-1 is optional visual polish that needs a spec decision, not a build fix.
- **Profile: Ship with fixes.** Update the spec for the mobile order (N2-02). PR-1 and PR-4 remain as polish.
- **Security: Ship with fixes.** Add the passkey nudge (N2-04). SE-2 is polish.
- **Confirm it’s you: Ship with fixes.** N2-03 and N2-05. ST-2 is polish.
- **Data and privacy: Ship with fixes.** N2-01.
- **Deletion lifecycle: Ship with fixes.** N2-06, N2-07, N2-08.

## R2.7 What improved most

1. **Recovery is now designed rather than missing.** Screen C turns the lost-mailbox dead end into a clear, honest route, and its keyboard and focus rules are specified.
2. **The deletion lifecycle has no dead ends.** It has one token rule, three entry points to the export, an account chip on the pending page, and a return URL on Keep.
3. **The fix pass was precise.** Skeleton heights now match exactly, every copy fix landed, and the hand-off and keyboard rules were written into the spec.

The new issues are almost all places where a board and the spec now disagree (N2-01, N2-02, N2-05, N2-06). Before handoff, run one pass that checks each changed board against the spec text.

---

# Round 3 (2026-11-05)

Short re-check of the team’s polish pass: canvas version `1793877569-bad9` (56 boards) and spec Revision 5. Same independence, rubric, weights and ship bar; same exclusions.

## R3.1 Verdict

**Overall: 8.7 / 10, Ship with fixes.**

- Every open round 2 item is closed, except N2-02, which is partly done.
- One new Minor (**N3-01**) keeps Profile at Ship with fixes.
- The other seven units **Ship**.
- Nothing blocks handoff.

| Unit | Round 2 | Round 3 | Verdict |
|---|---|---|---|
| Account shell | 8.7 | 8.7 (kept) | Ship |
| Profile | 8.4 | **8.5** | Ship with fixes (N3-01) |
| Sign-in and security | 8.6 | **8.7** | Ship |
| Confirm it’s you | 8.7 | **8.9** | Ship |
| Preferences | 8.6 | **8.6** | Ship |
| Connected apps | 8.9 | 8.9 (kept) | Ship |
| Data and privacy | 8.2 | **8.5** | Ship |
| Deletion lifecycle | 8.5 | **8.8** | Ship |

## R3.2 Method

**Sync check.** All 56 boards, `account.css` (91,760 bytes) and `canvas.json` on disk match the published file list.

**Builds:** `.design/qa-out/eval-3/`, plus the forced-dark copy `eval-3-dark/`.

**Change map** (diff against round 2’s build):

- 12 boards changed: the delete dialog and its mobile wrapper; `Security-no-passkeys`; `Signin-deleted`; the five step-up boards (`Stepup`, `Stepup-code`, `-code-error`, `-code-locked`, `-cant-get-code`) and their mobile and dark wrappers.
- `account.css` changed: the compact Profile preview on phones.

**Automated pass** (port 9861; 10 viewports × 2 themes): 0 blockers, 3 majors, 777 minors. All three majors are the false positives already verified in round 2:

- the intended 2-line name clamp on Profile-extreme (×2);
- the locked-input group ring on `Stepup-code-locked`.

The round 2 delete-dialog overlap is gone.

**Manual pass** (port 9862):

- Screenshots of Profile at 390, 320 (dark) and 768, of the Profile states, the delete dialog (1280, 390, 844×390, 320 dark), step-up A and C, and the passkeys empty state.
- `#email-title` measured across five Profile states at 390, 320 and 1280.
- Spec §6.2 checked against `canvas.json`: all 56 boards listed, no TBD.

Every server and browser was shut down. Evidence is under `.design/qa-out/eval-3-results/`.

## R3.3 Round 2 items: status

| ID | Status | Evidence |
|---|---|---|
| N2-01 | **Resolved** | The delete dialog now shows the consequences list, then the typed field (`shots/Data-delete-confirm@1280x800.png`) |
| N2-02 | **Partly** | The fields and hint stay together, and a compact reply card follows the form (`shots/Main@390x844@full.png`). But the CSS differs from the spec; see N3-01 |
| N2-03 | **Resolved** | The passkey bullet is `data-when="has-passkey"`, and the spec shows it "Only when the person has a passkey". The title clause is fixed (line 847) |
| N2-04 | **Resolved** | "Add a passkey so you can still sign in if you lose access to your email." (`Security-no-passkeys`; spec line 760) |
| N2-05 | **Resolved** | One rule for every entry path (§7.4 table, §4.0 hand-off step 3): screen A’s title, screen B’s code input, screen C’s title. The `Stepup` title has `tabindex="-1"` and no arrival ring; `Stepup-code` draws focus on the input |
| N2-06 | **Resolved** | `Signin-deleted` → `Export-start` (`Signin-deleted.dc.html:20`) |
| N2-07 | **Resolved** | "A live account wins" (spec line 1117) |
| N2-08 | **Resolved** | The trade-off is stated as accepted, with a rate limit of 5 per address and 20 per IP per hour, shown as a countdown button (spec line 1073, §9 Q8) |
| X-1 | **Resolved** | One source of truth per concern (line 1473: `account.css` for values, the spec for behaviour, copy and states). Review IDs moved to §11, with none before line 1727. §6.2 matches `canvas.json` |
| PR-1, PR-4, SE-2, ST-2, PF-1 | **Resolved (accepted rationale)** | Spec §1.5 rule 8, "Why empty space is kept"; see R3.4 |
| DP-5 | **Resolved** | The board order now matches the spec, and the field scrolls into view on open (spec line 1009) |

## R3.4 The reserved-space rationale

**Acceptable.** Rule 8 now does four things:

- states the rule;
- names every place it shows (the name form, section actions, step-up screen A, Preferences, change-email step 1);
- gives the benefit: saving, an error or a status never pushes content someone is reading or about to tap;
- tells engineers not to collapse it.

It is applied consistently and holds when measured. `#email-title` stays at 830 (390), 876 (320) and 676 (1280) across clean, saved, save-failed and photo-removed. The only move is the documented field-error exception (+28).

The cost is a looser rhythm: about 150 px under the desktop name hint, and about 125 px between Preferences sections at 320. That is a deliberate, documented trade, not a defect, so these five items are closed.

An optional later refinement: draw Cancel and Save as unavailable in the clean name form. That gives the largest gap content without breaking the rule.

## R3.5 New issue

| ID | Unit | Severity | Issue | Evidence |
|---|---|---|---|---|
| N3-01 | Profile | Minor | **On phones the compact preview’s CSS differs from the spec.** Spec §1.4 (line 232) and §4.2 (line 632) keep a **visible** "How customers see you" heading and caption, and place the action row *with its alert slot* before the preview, about 150 px under First name. The CSS does three things differently: (1) it visually hides the caption, so an unlabelled chat card floats under the form; (2) it moves the alert slot after the card, so a save failure appears below it, about 180 px from Save (Save y ≈ 582, alert y ≈ 760 at 390); (3) the bubble starts about 250 px under the field. Build it as specified: a visible caption, and the alert directly under the action row. | `shots/Main@390x844@full.png`, `shots/Profile-save-failed@390x844@full.png`; account.css `@container (max-width: 619px)` block |

## R3.6 Scorecards (re-graded units)

| Unit | Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Score** |
|---|---|---|---|---|---|---|---|---|---|
| Profile | 8.0 | 8.5 | 8.5 | 9.0 | 8.5 | 8.5 | 9.0 | 8.0 | **8.5** |
| Sign-in and security | 8.0 | 9.0 | 8.5 | 9.0 | 8.5 | 8.5 | 9.0 | 9.0 | **8.7** |
| Confirm it’s you | 8.0 | 9.0 | 9.0 | 9.0 | 9.0 | 8.5 | 9.5 | 9.0 | **8.9** |
| Preferences | 7.5 | 9.0 | 8.5 | 9.0 | 9.0 | 8.5 | 8.5 | 9.0 | **8.6** |
| Data and privacy | 8.0 | 9.0 | 8.5 | 9.0 | 8.5 | 8.5 | 8.0 | 8.5 | **8.5** |
| Deletion lifecycle | 8.5 | 9.0 | 9.0 | 8.5 | 8.5 | 8.5 | 9.0 | 9.0 | **8.8** |

Account shell (8.7) and Connected apps (8.9) are unchanged and keep their round 2 grades.

**What changed in the scores:**

- **Profile:** visual and responsive rose for the tidier phone layout. Handoff stays at 8.0 because of N3-01.
- **Preferences:** visual stays at 7.5 because the page looks the same; the rationale closes PF-1 without changing how it looks.
- **The other four** rose on consistency and handoff, now that the boards and spec agree.

**Overall:** 69.6 / 8 = **8.7**.

---

# Round 4 (2026-11-05): Profile only

Re-check of the N3-01 fix: canvas version `1793877977-4d5d`, in which only `account.css` changed (90,230 bytes, matching the published file), and spec Revision 5 (§1.4, §4.2 and §7.2 row 24 updated). Same independence and ship bar.

**Method:**

- Rebuilt into `.design/qa-out/eval-4/`, plus a forced-dark copy.
- Board markup is unchanged from round 3 (0 boards differ); only the phone-layout CSS changed.
- Rig on all 12 Profile boards at 390, 375, 320, 768 and 1280, light and dark (port 9871), and on the forced-dark build at 390, 375, 320 and 768 (port 9873). Result: no overflow, no new findings. The only "major" is the intended sidebar name clamp on Profile-extreme.
- My own screenshots and measurements (port 9872).
- Every server and browser was shut down. Evidence is in `.design/qa-out/eval-4-results/`.

## Status

| ID | Status | Evidence |
|---|---|---|
| N3-01 | **Resolved** | The CSS reorder is gone, so the DOM order is now the visual order. The caption ("How customers see you" and its description) is visible. The save error sits directly under Save (`shots/Profile-save-failed@390x844@full.png`), and the CSS, the spec (§1.4 lines 233–241, §4.2 line 641, §7.2 row 24) and the boards agree |
| N2-02 | **Resolved** | The fields and hint stay together, and the compact, labelled preview follows the form at 390, 375, 320 and 768, in both themes (`shots/Main@390x844@full.png`, `Main@375x812@dark@full.png`, `Main@320x640@full.png`, `Main@768x1024@dark@full.png`) |

**No regression:**

- `#email-title` holds across clean, saved, save-failed and photo-removed at every width: 962 (390), 984 (375), 1028 (320), 870 (768), 676 (1280). The only move is the documented field-error exception (+28).
- Desktop is unchanged.
- No other unit uses the changed rules, so they keep their round 3 grades.

## The stated rationale

**Acceptable.** On phones the preview now sits about 420 px under First name (`.ra-msgr` y = 784 vs the field at 360 at 390), so it can be under the on-screen keyboard while you type. The spec now says so plainly, rather than claiming otherwise:

> a labelled preview, and errors right next to Save, matter more than keeping the preview in view above the on-screen keyboard. The preview confirms a change; it isn’t needed to complete the task.

That is the right priority. Error proximity and a labelled control are accessibility and usability needs; the live preview is a confirmation that remains visible on desktop and after the keyboard closes.

## Profile re-grade

| Visual | Consistency | Usability | Content | A11y | Responsive | States | Handoff | **Score** | Verdict |
|---|---|---|---|---|---|---|---|---|---|
| 8.0 | 9.0 | 8.5 | 9.0 | 9.0 | 8.5 | 9.0 | 9.0 | **8.7** | **Ship** |

Consistency, accessibility and handoff rose because the DOM order now matches the visual order and the spec, CSS and boards agree.

**Overall: 8.7 / 10, Ship.** All 8 units ship. The other unit scores are as in round 3: Shell 8.7, Security 8.7, Step-up 8.9, Preferences 8.6, Connected apps 8.9, Data and privacy 8.5, Lifecycle 8.8.
