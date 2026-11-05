# WP4.3 colour contrast audit (Phase 4)

Tester: WP4.3. Measured, not fixed. No board, stylesheet, commit or publish was touched. Boards were built into `.design/edges/out-p4-contrast/` (light) and `.design/edges/out-p4-contrast-dark/` (dark, `BOARD_THEME=dark`) and are git-ignored.

## Result

**0 failures. 0 blockers, 0 minors.** Every measured text, control boundary, placeholder and focus indicator on all 73 boards meets its WCAG 2.2 AA threshold in both themes at both viewports. The lowest ratios are 3.30:1 (the `ok` badge disc, against a 3:1 threshold) and 4.31:1 (focus ring on the grey sidebar). Nothing sits within 0.1 above a threshold, so there are no borderline items to list.

Two items outside the audit's selectors are noted under "Not measured" below. Neither is a failure in the values I did measure, but both should be checked by hand before handoff.

## Method

- **Boards:** all 73 fluid boards from `index.json` (`*.fixed.html` skipped). Built with `node qa-rig/build.mjs edges/project <out> edges/blobs`, and the dark build with `BOARD_THEME=dark`.
- **Serving:** `python3 -I -m http.server 8823 --bind 127.0.0.1`, one out-dir at a time. Each server's identity was checked with `cmp` against a known board before the run. A first attempt hit a port race; its results were discarded and both themes were re-run.
- **Themes:** light build with `colorScheme: light`, base page `#ffffff`. Dark build with `colorScheme: dark`, base page `#000000`.
- **Viewports:** 390×844 and 1440×900.
- **Text:** every visible element with direct non-whitespace text, skipping `[inert]`, `[aria-hidden="true"]` and `.ra-sr` as specified. Colour is computed `color`, with `opacity` on the ancestor chain folded into alpha. The background is the first non-transparent `background-color` walking up from the element (its own paint included), with alpha layers blended over the base page colour. Threshold 4.5:1, or 3:1 when the size is at least 24 px, or at least 18.66 px and weight at least 700.
- **Placeholders:** computed `::placeholder` colour on each visible `input`/`textarea` with a placeholder, against the field's effective background. Threshold 4.5:1.
- **Non-text:** every visible `input` (not `hidden`), `textarea`, `[data-slot=button][data-variant=outline]`, `[data-slot=switch]` and `.ri-badge`. Compared against the background outside the control, not its own fill. Threshold 3:1.
  - Inputs and outline buttons use the border colour, falling back to "not measured" when there is no border.
  - Switches use the track border and track fill, whichever is present.
  - Badges use the disc (background) and the border, plus the glyph: the `svg` is drawn in `currentColor`, so the glyph colour is the badge's computed colour, measured on the disc.
  - Badges are `aria-hidden` decorative icons but meaningful status marks, so they were measured even though `aria-hidden` subtrees are skipped for text. Non-text skips only `[inert]` and `.ra-sr`.
- **Focus:** Tab three times from the top of each board, with a 700 ms wait after each Tab so the `var(--ra-dur)` transitions on input borders and box-shadows settle. Indicator colour is the `outline-color` (when the outline is visible) or the first colour of `box-shadow`, compared with the background outside the element. Threshold 3:1.
- **Recording:** every measured pair is kept with its ratio and where it occurred. Unique pairs are summarised below with their lowest ratio in each theme. Raw data is in the scratchpad (`wp43/light.json`, `wp43/dark.json`).

Coverage in both themes: 2,542 text items, 32 placeholders, 482 control boundaries (inputs, outline buttons, switches and badge discs and glyphs), 424 focus stops. Text-bearing elements skipped by the `[inert]`/`[aria-hidden]`/`.ra-sr` rule: 967. No background images were found on the paint path, so no pair was unmeasurable for that reason. No SVG text was found.

## Failures

No failures. The table is empty: no CON IDs were raised, and no owner needs a fix.

The 8 Tab-3 stops that land on no element are listed under "Observations", not here.

## Observations (not failures)

- **Lowest passing items** (none within 0.1 of a threshold): the `ok` badge disc `#16a34a` on white, 3.30:1 (Recover-canceled, both themes, both viewports) and the same disc's glyph; the outline button border `#86868b` on the grouped-list grey `#f5f5f7`, 3.33:1 (Profile, Profile-email-changed, Security); the dark outline border `#6e6e73` on `#1d1d1f`, 3.32:1 (Profile-email-code-mobile dialog footer, Cancel).
- **Focus rings:** 424 focus stops show an indicator. The lowest is the 2 px outline `#0071e3` on `#f5f5f7` at 4.31:1 (sidebar skip link and sidebar items). In dark, the lowest is `#2997ff` on `#1d1d1f` at 5.58:1 (Profile-email-code-mobile dialog).
- **Tab-3 with no focusable element:** on Security-remove-failed, Security-unlink, Session-expired and Session-expired-mobile (both viewports), the third Tab leaves `document.activeElement` on `body`. This is a keyboard-order question, not a contrast one, but it may mean a focus stop is missing or the dialog traps focus incorrectly. Worth checking with the keyboard pass in the QA rig.
- **Aria-disabled countdowns** (Limited, Recover-declined, Recover-limited, Signin-limited, Signin-redirecting): the text is measured as text. Lowest is `#6e6e73` on white at 5.07:1 (light), and `#a1a1a6` on black at 8.16:1 (dark). These meet the 4.5:1 text rule.

## Not measured (outside the audit's selectors)

1. **OTP slot borders.** `input#code`, `input#email-code` and `input#link-code` are visually hidden (the rig reports "no border"). The visible `[data-slot=input-otp-slot]` boxes are not in the selector list. I probed them by hand on Recover-code, light only: an inactive slot border `#86868b` on the field fill `#f5f5f5` is 3.32:1, and against white 3.62:1. The active slot `#0071e3` on `#f5f5f5` is 4.31:1. All pass 3:1. Dark slots were not probed.
2. **Typed text in fields.** Input values use the input's `color`, which was not measured. Placeholders were measured.
3. **Logos and decorative tiles.** Google, Microsoft and the shiba are exempt. The brand tile edge is decorative and not in the audit's selector list; its values are reported in the cross-check below.

## Pair summary

Light build (34 unique pairs):

| Foreground | Background | Lowest ratio | Threshold | Used for | Lowest at (board, viewport, element) |
|---|---|---|---|---|---|
| `#16a34a` | `#ffffff` | 3.30:1 | 3:1 | badge disc (background) | Recover-canceled 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok "span" |
| `#ffffff` | `#16a34a` | 3.30:1 | 3:1 | badge glyph (currentColor) on disc | Recover-canceled 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok > svg "badge glyph" |
| `#86868b` | `#f5f5f7` | 3.33:1 | 3:1 | outline button border | Profile 390x844 ul[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-actions] > button[data-slot=button] "Change email" |
| `#6e6e73` | `#161617` | 3.57:1 | 3:1 | outline button border | Security-dark 390x844 ul.has-media[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-actions] > button[data-slot=button] "Rename" |
| `#86868b` | `#ffffff` | 3.62:1 | 3:1 | outline button border, field border | Error-link 390x844 div.ri-col > div.ri-body > div.ri-actions > a[data-slot=button] "Sign in" |
| `#6e6e73` | `#000000` | 4.14:1 | 3:1 | field border, outline button border | Main-dark 390x844 input#email "input" |
| `#0071e3` | `#f5f5f7` | 4.31:1 | 3:1 | focus ring | Account-sidebar 390x844 div.ra.ra--part > a.ra-skip[data-slot=button] "Skip to content" |
| `#6e6e73` | `#f5f5f5` | 4.64:1 | 4.5:1 | placeholder | Main 390x844 input#email "placeholder: name@example.com" |
| `#6e6e73` | `#f5f5f7` | 4.66:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > dl.ri-rec-mail-env > dd "Richie" |
| `#ffffff` | `#0071e3` | 4.70:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > main.ri-rec-mail-card > p.ri-rec-mail-cta > a[data-slot=button] "This wasn’t me" |
| `#0071e3` | `#ffffff` | 4.70:1 | 3:1 | focus ring, field border, text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > main.ri-rec-mail-card > p.ri-rec-mail-cta > a[data-slot=button] "This wasn’t me" |
| `#6e6e73` | `#ffffff` | 5.07:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > main.ri-rec-mail-card > div.ri-rec-mail-fine > p "You’re getting this because this address is the email on a R" |
| `#0066cc` | `#f5f5f7` | 5.11:1 | 4.5:1 | text | Security 390x844 ul.has-media[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-actions] > a[data-slot=button] "Change email" |
| `#0066cc` | `#ffffff` | 5.57:1 | 4.5:1 | text | Email-recovery-alert 390x844 main.ri-rec-mail-card > div.ri-rec-mail-fine > p > a "https://account.richie.example/recover/cancel?t=b7Kx2mQp9vRw" |
| `#d0000a` | `#ffffff` | 5.70:1 | 4.5:1 | text | Profile-email-taken 390x844 p#new-email-error > span "That email already belongs to another Richie account. Use a " |
| `#b31917` | `#fae6e7` | 5.70:1 | 4.5:1 | text | Security 390x844 section#sessions > div.ra-section-foot > button[data-slot=button] "Sign out of other devices" |
| `#2997ff` | `#161617` | 6.00:1 | 4.5:1 | text, focus ring | Security-dark 390x844 ul.has-media[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-actions] > a[data-slot=button] "Change email" |
| `#ff7263` | `#33110e` | 6.39:1 | 4.5:1 | text | Security-dark 390x844 section#sessions > div.ra-section-foot > button[data-slot=button] "Sign out of other devices" |
| `#ff5447` | `#000000` | 6.61:1 | 4.5:1 | text | Signin-offline-dark 390x844 div.ri-commit > div.ri-slot > p.ra-status.is-error > span "We couldn’t send a code because you’re offline." |
| `#a1a1a6` | `#1a1a1a` | 6.80:1 | 4.5:1 | placeholder | Main-dark 390x844 input#email "placeholder: name@example.com" |
| `#2997ff` | `#000000` | 6.96:1 | 4.5:1 | text, focus ring | Error-dark 390x844 div.ri-body > div.ri-edge-ref > p.ri-ref > button[data-slot=button] "Copy" |
| `#a1a1a6` | `#161617` | 7.03:1 | 4.5:1 | text | Security-dark 390x844 ul.has-media[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-content] > p[data-slot=item-description] "Always on. Each code works once and expires after 5 minutes." |
| `#a1a1a6` | `#000000` | 8.16:1 | 4.5:1 | text | Error-dark 390x844 main.ri-main > div.ri-col > div.ri-head > p.ri-lede "It’s a problem on our side, not with your account. Go to Ric" |
| `#1d1d1f` | `#4ade80` | 9.66:1 | 3:1 | badge glyph (currentColor) on disc | Signedout-dark 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok > svg "badge glyph" |
| `#4ade80` | `#000000` | 12.05:1 | 3:1 | badge disc (background) | Signedout-dark 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok "span" |
| `#f5f5f7` | `#2d2d2e` | 12.58:1 | 4.5:1 | text | Security-dark 1440x900 ul[data-slot=sidebar-menu] > li[data-slot=sidebar-menu-item] > a[data-slot=sidebar-menu-button] > span.ra-trunc "Sign-in and security" |
| `#1d1d1f` | `#e6e6e8` | 13.54:1 | 4.5:1 | text | Profile 1440x900 ul[data-slot=sidebar-menu] > li[data-slot=sidebar-menu-item] > a[data-slot=sidebar-menu-button] > span.ra-trunc "Profile" |
| `#1d1d1f` | `#f5f5f5` | 15.41:1 | 4.5:1 | text | Recover-limited 390x844 textarea#more "I changed jobs, so I can’t get email at my old work address " |
| `#1d1d1f` | `#f5f5f7` | 15.46:1 | 4.5:1 | text | Account-sidebar 390x844 div[data-slot=sidebar-container] > nav.ra-side-nav > a.ra-side-back[data-slot=sidebar-menu-button] > span.ra-trunc "Back to Richie" |
| `#f5f5f7` | `#161617` | 16.61:1 | 4.5:1 | text | Security-dark 390x844 ul.has-media[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-content] > p.wrap-2[data-slot=item-title] "MacBook Pro" |
| `#1d1d1f` | `#ffffff` | 16.83:1 | 3:1 | text, badge disc (background) | Email-recovery-alert 390x844 h1#page-title "Someone asked to change the email on your Richie account" |
| `#ffffff` | `#1d1d1f` | 16.83:1 | 3:1 | badge glyph (currentColor) on disc | Error 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-neutral > svg "badge glyph" |
| `#f5f5f7` | `#000000` | 19.29:1 | 3:1 | text, badge disc (background) | Error-dark 390x844 h1#page-title "We couldn’t sign you in" |
| `#000000` | `#f5f5f7` | 19.29:1 | 3:1 | badge glyph (currentColor) on disc | Error-dark 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-neutral > svg "badge glyph" |

Dark build (24 unique pairs):

| Foreground | Background | Lowest ratio | Threshold | Used for | Lowest at (board, viewport, element) |
|---|---|---|---|---|---|
| `#6e6e73` | `#1d1d1f` | 3.32:1 | 3:1 | outline button border | Profile-email-code-mobile 390x844 div[data-slot=dialog-content] > form.ra-dialog-form > div[data-slot=dialog-footer] > button[data-slot=button] "Cancel" |
| `#6e6e73` | `#161617` | 3.57:1 | 3:1 | outline button border | Profile 390x844 ul[data-slot=item-group] > li[data-slot=item] > div[data-slot=item-actions] > button[data-slot=button] "Change email" |
| `#6e6e73` | `#000000` | 4.14:1 | 3:1 | outline button border, field border | Error-link 390x844 div.ri-col > div.ri-body > div.ri-actions > a[data-slot=button] "Sign in" |
| `#ffffff` | `#0071e3` | 4.70:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > main.ri-rec-mail-card > p.ri-rec-mail-cta > a[data-slot=button] "This wasn’t me" |
| `#ff7263` | `#4a2827` | 4.82:1 | 4.5:1 | text | Security-remove-failed 1440x900 div[data-slot=alert-dialog-footer] > button.is-focused[data-slot=button] > span.ra-steps > span "Remove passkey" |
| `#a1a1a6` | `#343435` | 4.86:1 | 4.5:1 | placeholder | Profile-email-taken 390x844 input#new-email "placeholder: name@example.com" |
| `#ff5447` | `#1d1d1f` | 5.30:1 | 4.5:1 | text | Profile-email-taken 390x844 p#new-email-error > span "That email already belongs to another Richie account. Use a " |
| `#2997ff` | `#1d1d1f` | 5.58:1 | 4.5:1 | text, focus ring, field border | Profile-email-code-mobile 390x844 div.ra-steps > div.ra-dialog-pane > div.ra-center > button[data-slot=button] "Use a different email" |
| `#2997ff` | `#161617` | 6.00:1 | 3:1 | focus ring, text | Account-sidebar 390x844 div.ra.ra--part > a.ra-skip[data-slot=button] "Skip to content" |
| `#ff7263` | `#33110e` | 6.39:1 | 4.5:1 | text | Security 390x844 section#sessions > div.ra-section-foot > button[data-slot=button] "Sign out of other devices" |
| `#a1a1a6` | `#1d1d1f` | 6.54:1 | 4.5:1 | text | Profile-email-code-mobile 390x844 p#email-code-desc "Enter the code we sent to ." |
| `#ff5447` | `#000000` | 6.61:1 | 4.5:1 | text | Security-link-error 390x844 div.ra-section-foot > div.ra-slot > p.ra-status.is-error > span "That Microsoft account is linked to another Richie account." |
| `#a1a1a6` | `#1a1a1a` | 6.80:1 | 4.5:1 | placeholder | Main 390x844 input#email "placeholder: name@example.com" |
| `#2997ff` | `#000000` | 6.96:1 | 4.5:1 | text, focus ring | Email-recovery-alert 390x844 main.ri-rec-mail-card > div.ri-rec-mail-fine > p > a "https://account.richie.example/recover/cancel?t=b7Kx2mQp9vRw" |
| `#a1a1a6` | `#161617` | 7.03:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > dl.ri-rec-mail-env > dd "Richie" |
| `#a1a1a6` | `#000000` | 8.16:1 | 4.5:1 | text | Email-recovery-alert 390x844 div.ra.ri-rec-mail > main.ri-rec-mail-card > div.ri-rec-mail-fine > p "You’re getting this because this address is the email on a R" |
| `#1d1d1f` | `#4ade80` | 9.66:1 | 3:1 | badge glyph (currentColor) on disc | Recover-canceled 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok > svg "badge glyph" |
| `#4ade80` | `#000000` | 12.05:1 | 3:1 | badge disc (background) | Recover-canceled 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-ok "span" |
| `#f5f5f7` | `#2d2d2e` | 12.58:1 | 4.5:1 | text | Profile 1440x900 ul[data-slot=sidebar-menu] > li[data-slot=sidebar-menu-item] > a[data-slot=sidebar-menu-button] > span.ra-trunc "Profile" |
| `#f5f5f7` | `#1d1d1f` | 15.46:1 | 4.5:1 | text | Profile-email-code-mobile 390x844 h2#email-code-title "Check your new email" |
| `#f5f5f7` | `#1a1a1a` | 16.07:1 | 4.5:1 | text | Recover-limited 390x844 textarea#more "I changed jobs, so I can’t get email at my old work address " |
| `#f5f5f7` | `#161617` | 16.61:1 | 4.5:1 | text | Account-sidebar 390x844 div[data-slot=sidebar-container] > nav.ra-side-nav > a.ra-side-back[data-slot=sidebar-menu-button] > span.ra-trunc "Back to Richie" |
| `#f5f5f7` | `#000000` | 19.29:1 | 3:1 | text, badge disc (background) | Email-recovery-alert 390x844 h1#page-title "Someone asked to change the email on your Richie account" |
| `#000000` | `#f5f5f7` | 19.29:1 | 3:1 | badge glyph (currentColor) on disc | Error 390x844 div.ri-col > div.ri-head > div.ri-mark > span.ri-badge.is-neutral > svg "badge glyph" |

## Cross-check against contrast-report.md and spec Part A §6.3

I recalculated each pair from its hex values with the WCAG formula, and compared the result with the spec and WP3.1 values. "Measured" is the lowest ratio the DOM audit found for that exact pair (where it appears on a board).

| Pair | Light (spec / recalc / measured) | Dark (spec / recalc / measured) | Result |
|---|---|---|---|
| Ok badge glyph on disc | `#ffffff` on `#16a34a`: 3.30 / 3.30 / 3.30 | `#1d1d1f` on `#4ade80`: 9.66 / 9.66 / 9.66 | Match |
| Ok badge disc vs page | `#16a34a` on `#ffffff`: 3.30 / 3.30 / 3.30 | `#4ade80` on `#000000`: 12.05 / 12.05 / 12.05 | Match |
| Neutral badge glyph on disc | `#ffffff` on `#1d1d1f`: 16.83 / 16.83 / 16.83 | `#000000` on `#f5f5f7`: 19.29 / 19.29 / 19.29 | Match |
| Neutral badge disc vs page | `#1d1d1f` on `#ffffff`: 16.83 / 16.83 / 16.83 | `#f5f5f7` on `#000000`: 19.29 / 19.29 / 19.29 | Match |
| Method button label | `#1d1d1f` on `#ffffff`: 16.83 / 16.83 / 16.83 | `#f5f5f7` on `#000000`: 19.29 / 19.29 / 19.29 | Match |
| Method button edge vs page | `#86868b` on `#ffffff`: 3.62 / 3.62 / 3.62 | `#6e6e73` on `#000000`: 4.14 / 4.14 / 4.14 | Match |
| "or", step line, notes (Slate) | `#6e6e73` on `#ffffff`: 5.07 / 5.07 / 5.07 | `#a1a1a6` on `#000000`: 8.16 / 8.16 / 8.16 | Match |
| Reference code, footer "Richie" (Ink) | `#1d1d1f` on `#ffffff`: 16.83 / 16.83 / 16.83 | `#f5f5f7` on `#000000`: 19.29 / 19.29 / 19.29 | Match |
| White glyph on dark ok disc (rejected) | `#ffffff` on `#4ade80`: 1.74 / 1.74 / not on DOM (rejected in design) | | Match |
| Brand tile edge (decorative) | `#d2d2d7` on `#f5f5f7`: 1.38 / 1.38 / not on DOM | `#424245` on `#161617`: 1.81 / 1.81 / not on DOM | Match, not in scope |
| Google stroke (reference) | `#747775` on `#ffffff`: 4.53 / 4.53 / not on DOM | `#8e918f` on `#131314`: 5.83 / 5.83 / not on DOM | Match, logo reference |

Every spec and report value agrees with my recalculation to two decimals, and every value that appears on a board also agrees with the DOM measurement. No value differs by more than 0.05, so no spec values need correction. Rows marked "not on DOM" are decorative or logo colours outside the audit's selectors, or the rejected colour; they were not measured.
