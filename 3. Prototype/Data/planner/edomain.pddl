(define(domain event)
(:requirements :strips :fluents :typing)
(:types
  eo - object 
)
(:predicates
(ObjectName11 ?eo)
(ObjectName38 ?eo)
(ObjectName8 ?eo)
(ObjectName22 ?eo)
(ObjectName35 ?eo)
(ObjectName36 ?eo)
(ObjectName14 ?eo)
(ObjectName15 ?eo)
(ObjectName27 ?eo)
(ObjectName26 ?eo)
(ObjectName9 ?eo)
(ObjectName28 ?eo)
(ObjectName6 ?eo)
(ObjectName32 ?eo)
(ObjectName31 ?eo)
(ObjectName12 ?eo)
(ObjectName33 ?eo)
(ObjectName16 ?eo)
(ObjectName29 ?eo)
(ObjectName34 ?eo)
(ObjectName1 ?eo)
)

;;**********action_9228_3054_3413__2327_7068_3309_8314*************
(:action action_9228_3054_3413__2327_7068_3309_8314
:parameters ( ?ObjectValue11 - eo ?ObjectValue9 - eo ?ObjectValue16 - eo ?ObjectValue18 - eo ?ObjectValue24 - eo ?ObjectValue31 - eo ?ObjectValue25 - eo ?ObjectValue2 - eo ?ObjectValue32 - eo ?ObjectValue38 - eo ?ObjectValue33 - eo ?ObjectValue26 - eo ?ObjectValue27 - eo ?ObjectValue19 - eo ?ObjectValue1 - eo ?ObjectValue35 - eo ?ObjectValue39 - eo ?ObjectValue5 - eo ?ObjectValue3)
:precondition
(and 
 (ObjectName11 ?ObjectValue11) (ObjectName38 ?ObjectValue9) (ObjectName8 ?ObjectValue16) (ObjectName22 ?ObjectValue18) (ObjectName35 ?ObjectValue24) (ObjectName36 ?ObjectValue11) (ObjectName14 ?ObjectValue31) (ObjectName15 ?ObjectValue25) (ObjectName27 ?ObjectValue2) (ObjectName26 ?ObjectValue32) (ObjectName8 ?ObjectValue38) (ObjectName9 ?ObjectValue33)
)
:effect  
(and
 (ObjectName28 ?ObjectValue26) (ObjectName14 ?ObjectValue27) (ObjectName22 ?ObjectValue9) (ObjectName6 ?ObjectValue19) (ObjectName32 ?ObjectValue9) (ObjectName31 ?ObjectValue1) (ObjectName11 ?ObjectValue35) (ObjectName12 ?ObjectValue39) (ObjectName33 ?ObjectValue24) (ObjectName16 ?ObjectValue26) (ObjectName29 ?ObjectValue1) (ObjectName15 ?ObjectValue5) (ObjectName34 ?ObjectValue24) (ObjectName22 ?ObjectValue5) (ObjectName1 ?ObjectValue3) (ObjectName33 ?ObjectValue26)
)
)
)

